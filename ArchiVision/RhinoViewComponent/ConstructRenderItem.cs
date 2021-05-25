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
    public class ConstructRenderItem : BaseComponent
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
        public ConstructRenderItem()
          : base("ConstructRenderItem", "Nickname",
              "Description",  Subcategory.UI_RhinoView)
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
            Param_OGLShader param_OGLShader = new Param_OGLShader();
            param_OGLShader.SetPersistentData(new GH_Material(Color.White));
            pManager.AddParameter(param_OGLShader, "Material", "M", "The material override", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new RenderItemParameter());
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_GeometricGoo destination = null;
            GH_Material destination2 = null;
            if (DA.GetData(0, ref destination) && DA.GetData(1, ref destination2) && destination.IsValid)
            {
                if (!(destination is IGH_PreviewData))
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, destination.TypeName + " does not support previews");
                }
                else if (destination2.Value != null)
                {
                    DA.SetData(0, new RenderItem((IGH_PreviewData)destination, destination2));
                }
            }
        }
        #endregion
    }
}