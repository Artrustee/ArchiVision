/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Components;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino;
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
          : base("Shader Render Item", "S Ri", "Shader Render Item")
        {

        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "m", "Mesh to preview", GH_ParamAccess.item);
            pManager.HideParameter(0);
            AddShaderParam(pManager);

            pManager.AddAngleParameter("AngleLimit", "A", "AngleLimit", GH_ParamAccess.item, Math.PI / 6);

            pManager.AddParameter(new CurveRenderAttributeParameter(), "Naked Edge Attribute", "N", "Naked Edge Attribute", GH_ParamAccess.item);
            pManager[3].Optional = true;

            pManager.AddParameter(new CurveRenderAttributeParameter(), "Interior Edge Attribute", "I", "Interior Edge Attribute", GH_ParamAccess.item);
            pManager[4].Optional = true;

            pManager.AddParameter(new CurveRenderAttributeParameter(), "Sharp Edge Attribute", "S", "Sharp Edge Attribute. It will take a lot of time!", GH_ParamAccess.item);
            pManager[5].Optional = true;

            pManager.AddParameter(new CurveRenderAttributeParameter(), "Out Line Attribute", "O", "Out Line Attribute", GH_ParamAccess.item);
            pManager[6].Optional = true;
        }


        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Mesh mesh = null;
            GH_Material mate = null;

            if (DA.GetData(0, ref mesh) && DA.GetData(1, ref mate) && mate.IsValid)
            {
                CurveRenderAttribute naked = null;
                CurveRenderAttribute interior = null;
                CurveRenderAttribute sharp = null;
                CurveRenderAttribute outline = null;

                double angle = Math.PI / 6;
                DA.GetData(2, ref angle);
                if (((Param_Number)Params.Input[2]).UseDegrees)
                    angle = RhinoMath.ToRadians(angle);
                DA.GetData(3, ref naked); 
                DA.GetData(4, ref interior);
                DA.GetData(5, ref sharp);
                DA.GetData(6, ref outline);

                if (mate.Value != null)
                {
                    DA.SetData(0, new MeshRenderItem(mesh, mate, naked, interior, outline, sharp, angle));
                }
            }
        }
        #endregion
    }
}