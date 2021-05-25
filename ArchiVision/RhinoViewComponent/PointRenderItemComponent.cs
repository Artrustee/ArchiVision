/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArchiVision
{
    public class PointRenderItemComponent : BaseRenderItemComponent
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.PointRenderItemComponent_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("8251ec8e-9db4-4144-9783-8a9414d1b752");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the PointRenderItemComponent class.
        /// </summary>
        public PointRenderItemComponent()
          : base("Point Render Item", "Pt RI","Point Render Item")
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "Point", GH_ParamAccess.item);
            pManager.AddColourParameter("Colour", "C", "Colour", GH_ParamAccess.item, Color.White);
            pManager.AddNumberParameter("Radius", "R", "Radius", GH_ParamAccess.item, 2);
            pManager.AddIntegerParameter("Point Style", "S", "Point Style", GH_ParamAccess.item, 0);
            Param_Integer param = (Param_Integer)pManager[3];
            param.AddNamedValue(nameof(PointStyle.ActivePoint), (int)PointStyle.ActivePoint);
            param.AddNamedValue(nameof(PointStyle.ArrowTail), (int)PointStyle.ArrowTail);
            param.AddNamedValue(nameof(PointStyle.ArrowTip), (int)PointStyle.ArrowTip);
            param.AddNamedValue(nameof(PointStyle.Asterisk), (int)PointStyle.Asterisk);
            param.AddNamedValue(nameof(PointStyle.Chevron), (int)PointStyle.Chevron);
            param.AddNamedValue(nameof(PointStyle.Circle), (int)PointStyle.Circle);
            param.AddNamedValue(nameof(PointStyle.Clover), (int)PointStyle.Clover);
            param.AddNamedValue(nameof(PointStyle.ControlPoint), (int)PointStyle.ControlPoint);
            param.AddNamedValue(nameof(PointStyle.Heart), (int)PointStyle.Heart);
            param.AddNamedValue(nameof(PointStyle.Pin), (int)PointStyle.Pin);
            param.AddNamedValue(nameof(PointStyle.RoundActivePoint), (int)PointStyle.RoundActivePoint);
            param.AddNamedValue(nameof(PointStyle.RoundControlPoint), (int)PointStyle.RoundControlPoint);
            param.AddNamedValue(nameof(PointStyle.RoundSimple), (int)PointStyle.RoundSimple);
            param.AddNamedValue(nameof(PointStyle.Simple), (int)PointStyle.Simple);
            param.AddNamedValue(nameof(PointStyle.Square), (int)PointStyle.Square);
            param.AddNamedValue(nameof(PointStyle.Tag), (int)PointStyle.Tag);
            param.AddNamedValue(nameof(PointStyle.Triangle), (int)PointStyle.Triangle);
            param.AddNamedValue(nameof(PointStyle.X), (int)PointStyle.X);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Point point = null;
            Color color = Color.Empty;
            double radius = 0;
            int style = 0;

            DA.GetData(0, ref point);
            DA.GetData(1, ref color);
            DA.GetData(2, ref radius);
            DA.GetData(3, ref style);

            radius = Math.Max(radius, 0);
            PointStyle ptStyle = (PointStyle)style;

            this.Message = ptStyle.ToString();

            DA.SetData(0, new PointRenderItem(point, color, ptStyle, (float)radius));
        }
        #endregion
    }
}