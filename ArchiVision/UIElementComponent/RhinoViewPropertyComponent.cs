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
    public class RhinoViewPropertyComponent : ElementComponent
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
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
              "RhinoView Property", Subcategory.UI_Element)
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new UIElementParameter());
            pManager.AddParameter(Helper.CreateViewParam());
            pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            //base.RegisterOutputParams(pManager);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            UIElement element = null;
            ViewportInfo info = default(ViewportInfo);

            DA.GetData(0, ref element);
            bool setVP = DA.GetData(1, ref info);

            Action wrong = () =>
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input Element must be from RhinoView Element Component!");
            };
            if (element == null) wrong();
            if (!(element is Border)) wrong();
            Border border = element as Border;
            if (!(border.Child is UniformGrid)) wrong();
            UniformGrid grid = border.Child as UniformGrid;
            foreach (var item in grid.Children)
            {
                if (!(item is WindowsFormsHost)) wrong();
                WindowsFormsHost host = item as WindowsFormsHost;
                if (!(host.Child is ViewportControl)) wrong();
                RhinoViewport viewport = ((ViewportControl)host.Child).Viewport;

                if(setVP) viewport.SetViewProjection(info, true);

                ((ViewportControl)host.Child).Refresh();
            }

            //Helper.FindWindow(element).UpdateLayout();

        }
        #endregion
    }
}