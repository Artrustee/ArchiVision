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
using System.Windows.Forms.Integration;

namespace ArchiVision
{
    public class RhinoViewElementComponent : ElementComponent
    {
        #region Values
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
              "RhinoView Element", nameof(Subcategory.UI_Element))
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(Helper.CreateViewParam());
            pManager[0].Optional = true;
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            WindowsFormsHost Element = new WindowsFormsHost() { Child = new ViewportControl() };
            RhinoViewport Viewport = ((ViewportControl)Element.Child).Viewport;

            ViewportInfo info = default(ViewportInfo);
            if (DA.GetData(0, ref info))
                Viewport.SetViewProjection(info, false);

            DA.SetData(0, Element);
        }
        #endregion
    }
}