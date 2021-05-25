/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArchiVision
{
    public class CurveRenderItemComponent : BaseRenderItemComponent
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.CurveRenderItemComponent_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("c8ce6db4-5412-44c3-9b66-f433c421a37a");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the CurveRenderItemComponent class.
        /// </summary>
        public CurveRenderItemComponent()
          : base("Curve Render Item", "Crv RI", "Curve Render Item")
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "Curve", GH_ParamAccess.item);
            pManager.AddColourParameter("Colour", "c", "Colour", GH_ParamAccess.item, Color.White);
            pManager.AddIntegerParameter("Thickness", "T", "Thickness", GH_ParamAccess.item, 2);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Curve curve = null;
            Color color = Color.Empty;
            int thick = 0;

            DA.GetData(0, ref curve);
            DA.GetData(1, ref color);
            DA.GetData(2, ref thick);

            DA.SetData(0, new CurveRenderItem(curve, color, thick));
        }
        #endregion
    }
}