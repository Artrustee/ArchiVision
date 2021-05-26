/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArchiVision
{
    public class CurveRenderItemAttributeComponent : BaseComponent
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.CurveRenderItemAttributeComponent_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("1f886b58-9e34-4cf4-9b43-07b8adf3c2b4");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the CurveRenderItemAttributeComponent class.
        /// </summary>
        public CurveRenderItemAttributeComponent()
          : base("CurveRenderItemAttributeComponent", "Nickname",
              "Description", Subcategory.UI_RhinoView)
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("Colour", "C", "Colour", GH_ParamAccess.item, Color.White);
            pManager.AddNumberParameter("Thickness", "t", "Thickness", GH_ParamAccess.item, 2);
            pManager.AddTextParameter("LineType", "T", "LintType", GH_ParamAccess.item, "");
            pManager.AddBooleanParameter("Absolute", "A", "Absolute", GH_ParamAccess.item, true);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new CurveRenderAttributeParameter());
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Color colour = Color.White;
            double thickness = 2;
            string lineT = "";
            bool abso = true;

            DA.GetData(0, ref colour);
            DA.GetData(1, ref thickness);
            DA.GetData(2, ref lineT);
            DA.GetData(3, ref abso);

            Linetype realtype = Rhino.RhinoDoc.ActiveDoc.Linetypes.FindName(lineT);
            this.Message = realtype.Name;

            DA.SetData(0, new CurveRenderAttribute(colour, thickness, realtype, abso));
        }
        #endregion
    }
}