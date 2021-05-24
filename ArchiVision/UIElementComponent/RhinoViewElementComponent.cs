/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using RhinoWindows.Forms.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms.Integration;

namespace ArchiVision
{
    public class RhinoViewElementComponent : ElementComponent
    {
        #region Values
        public bool ShowAxes { get; private set; }
        public DisplayModeDescription DisplayDesc
        {
            get
            {
                string name = "ArchiVision Display";
                var display = DisplayModeDescription.FindByName(name);
                if (display == null)
                {
                    display = DisplayModeDescription.GetDisplayMode(DisplayModeDescription.CopyDisplayMode(DisplayModeDescription.WireframeId, name));
                    display.DisplayAttributes.ShowAnnotations = false;
                    display.DisplayAttributes.ShowClippingPlanes = false;
                    display.DisplayAttributes.ShowCurves = false;
                    display.DisplayAttributes.ShowGrips = false;
                    display.DisplayAttributes.ShowIsoCurves = false;
                    display.DisplayAttributes.ShowLights = false;
                    display.DisplayAttributes.ShowPointClouds = false;
                    display.DisplayAttributes.ShowPoints = false;
                    display.DisplayAttributes.ShowRealtimeRenderProgressBar = false;
                    display.DisplayAttributes.ShowSurfaceEdges = false;
                    display.DisplayAttributes.ShowTangentEdges = false;
                    display.DisplayAttributes.ShowTangentSeams = false;
                    display.DisplayAttributes.ShowText = false;
                    display.DisplayAttributes.ViewSpecificAttributes.DrawWorldAxes = ShowAxes;

                    DisplayModeDescription.UpdateDisplayMode(display);

                }
                return display;
            }
        }
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.RhinoViewElementComponent_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("3947af1c-7bfb-4618-9828-2366549c095f");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the RhinoViewElementComponent class.
        /// </summary>
        public RhinoViewElementComponent()
          : base("RhinoView Element", "RV Ele",
              "RhinoView Element", Subcategory.UI_Element)
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Column Count", "CC", "Column Count", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("Viewport Count", "VC", "Viewport Count", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Border Inside Thickness", "BIT", "Border Inside Thickness", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Border Outside Thickness Addition", "BOT", "Border Outside Thickness Addition", GH_ParamAccess.item, 0);
            pManager.AddColourParameter("Border Color", "BC", "Border Color", GH_ParamAccess.item);
            pManager[4].Optional = true;
            pManager.AddBooleanParameter("Show World Axes", "SA", "Show World Axes", GH_ParamAccess.item, true);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int column = 1;
            int count = 1;
            double thickness = 0;
            double outThickness = 0;
            Color color = Color.Black;
            bool showWorldAxes = true;


            DA.GetData(0, ref column);
            DA.GetData(1, ref count);
            DA.GetData(2, ref thickness);
            DA.GetData(3, ref outThickness);
            DA.GetData(5, ref showWorldAxes);

            ShowAxes = showWorldAxes;
            thickness = Math.Max(thickness, 0);
            outThickness = Math.Max(outThickness, 0);

            if (column < 1 || count < 1) this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Count must larger than 1!");

            UniformGrid grid = new UniformGrid() { Columns = column,};

            for (int i = 0; i < count; i++)
            {
                ViewportControl vpControl = new ViewportControl();
                vpControl.Viewport.DisplayMode = this.DisplayDesc;

                grid.Children.Add(new WindowsFormsHost()
                {
                    Child = vpControl,
                    Margin = new Thickness(thickness/2),
                });
            }
            Border border = new Border() { Child = grid , Padding = new Thickness(outThickness + thickness/2) };
            if (DA.GetData(4, ref color)) border.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));

            DA.SetData(0, border);
        }
        #endregion
    }

    //class test : Rhino.Display.DisplayConduit
    //{
    //    protected override void PostDrawObjects(DrawEventArgs e)
    //    {
    //        base.PostDrawObjects(e);
    //    }
    //}
}