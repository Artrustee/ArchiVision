/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Components;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArchiVision.RhinoViewComponent
{
    public class ShaderRenderItemComponent : BaseRenderItemComponent
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.ConstructRenderItemComponent_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("a166595b-0503-4274-ba06-275f9cdfaccb");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the ConstructRenderItem class.
        /// </summary>
        public ShaderRenderItemComponent()
          : base("Shader Render Item", "C Ri", "Shader Render Item")
        {

        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry", "G", "Geometry to preview", GH_ParamAccess.item);
            pManager.HideParameter(0);
            AddShaderParam(pManager);
            pManager.AddBooleanParameter("Shader", "S", "Shader", GH_ParamAccess.item, true);
        }


        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_PreviewData geo = null;
            GH_Material mate = null;
            bool useShade = true;

            if (DA.GetData(0, ref geo) && DA.GetData(1, ref mate) && mate.IsValid)
            {
                DA.GetData(2, ref useShade);
                if (mate.Value != null)
                {
                    DA.SetData(0, new GeometryRenderItem(geo, mate, useShade));
                }
            }
        }
        #endregion
    }
}