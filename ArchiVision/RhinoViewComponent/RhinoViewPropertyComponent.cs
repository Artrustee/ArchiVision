/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Components;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using RhinoWindows.Forms.Controls;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms.Integration;

namespace ArchiVision
{
    public class RhinoViewPropertyComponent : ElementComponent
    {
        #region Values
        private BoundingBox _boundingBox;
        public override BoundingBox ClippingBox => _boundingBox;

        public Dictionary<string, List<RenderItem>> PreviewObjects { get; } = new Dictionary<string, List<RenderItem>>();

        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
        protected override Bitmap Icon => Properties.Resources.RhinoViewPropertyElementComponent_24_24;


        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("0536004e-0248-425c-8419-c72650cd8a73");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the RhinoViewPropertyComponent class.
        /// </summary>
        public RhinoViewPropertyComponent()
          : base("RhinoView Property", "Rv Prop",
              "RhinoView Property", Subcategory.UI_RhinoView)
        {
            this.Hidden = false;
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new UIElementParameter());
            pManager.AddParameter(Helper.CreateViewParam());
            pManager[1].Optional = true;
            pManager.AddParameter(new RenderItemParameter(GH_ParamAccess.tree));
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        public override void ClearData()
        {
            base.ClearData();
            PreviewObjects.Clear();
        }

        protected override void BeforeSolveInstance()
        {
            PreviewObjects.Clear();
            _boundingBox = BoundingBox.Empty;
        }


        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            UIElement element = null;
            ViewportInfo info = default(ViewportInfo);
            GH_Structure<RenderItemGoo> geos = null;


            DA.GetData(0, ref element);
            bool setVP = DA.GetData(1, ref info);
            bool addGeo = DA.GetDataTree(2, out geos);

            DoToViewport(element, (viewport, i) =>
            {
                if (addGeo)
                {
                    viewport.Name = "ArchiVision" + i.ToString();
                    if (i < geos.Branches.Count)
                    {
                        BoundingBox box = BoundingBox.Empty;

                        #region Get Preview Items.
                        List<RenderItem> preItems = new List<RenderItem>();
                        geos.Branches[i].ForEach((goo) =>
                        {
                            preItems.Add(goo.Value);
                            BoundingBox relay = ((IGH_GeometricGoo)goo.Value.Geometry).Boundingbox;
                            box.Union(relay);
                            _boundingBox.Union(relay);
                        });
                        #endregion
                        viewport.SetClippingPlanes(box);
                        PreviewObjects[viewport.Name] = preItems;
                    }
                    
                }
                if (setVP)
                {
                    viewport.SetViewProjection(info, true);
                }

            });

        }

        public void DoToViewport(UIElement element, Action<RhinoViewport, int> viewportAction)
        {
            if(!Helper.IsViewport(element, (host, view, inte) => viewportAction.Invoke(view, inte), true))
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input Element must be from RhinoView Element Component!");

        }

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            List<RenderItem> _items = FindItems(args);
            if (_items == null) return;
            _items.ForEach((item) => item.DrawViewportWires(args, base.Attributes.Selected));
        }

        public override void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            List<RenderItem> _items = FindItems(args);
            if (_items == null) return;
            _items.ForEach((item) => item.DrawViewportMeshes(args, base.Attributes.Selected));
        }

        private List<RenderItem> FindItems(IGH_PreviewArgs args)
        {
            if (PreviewObjects == null || args.Document.IsRenderMeshPipelineViewport(args.Display) || string.IsNullOrEmpty(args.Viewport.Name))
            {
                return null;
            }
            if (!PreviewObjects.Keys.Contains(args.Viewport.Name)) return null;

            return PreviewObjects[args.Viewport.Name];
        }

        public override void AppendRenderGeometry(GH_RenderArgs args)
        {
            GH_Document gH_Document = OnPingDocument();
            if (gH_Document != null && (gH_Document.PreviewMode == GH_PreviewMode.Disabled || gH_Document.PreviewMode == GH_PreviewMode.Wireframe))
            {
                return;
            }

            List<RenderItem> items = new List<RenderItem>();
            foreach (var item in PreviewObjects.Values)
            {
                if (item == null) continue;
                items.AddRange(item);
            }

            items = new List<RenderItem>(items);
            if (items.Count == 0)
            {
                return;
            }
            foreach (RenderItem item in items)
            {
                item.PushToRenderPipeline(args);
            }
        }

        #endregion
    }
}