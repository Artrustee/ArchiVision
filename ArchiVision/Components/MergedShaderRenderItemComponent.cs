///*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

//    Distributed under MIT license.

//    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
//*/

//using Grasshopper.Kernel;
//using Rhino.Geometry;
//using System;
//using System.Collections.Generic;
//using System.Drawing;

//namespace ArchiVision
//{
//    public class MergedShaderRenderItemComponent : BaseRenderItemComponent
//    {
//        #region Values
//        #region Basic Component info

//        public override GH_Exposure Exposure => GH_Exposure.primary;

//        /// <summary>
//        /// Provides an Icon for the component.
//        /// </summary>
//        protected override Bitmap Icon => Properties.Resources.MergedRenderItemComponent_24_24;

//        /// <summary>
//        /// Gets the unique ID for this component. Do not change this ID after release.
//        /// </summary>
//        public override Guid ComponentGuid => new Guid("b980896c-e690-4a6b-b8d0-1eb5461ada9d");


//        #endregion
//        #endregion

//        /// <summary>
//        /// Initializes a new instance of the MergedShaderRenderItemComponent class.
//        /// </summary>
//        public MergedShaderRenderItemComponent()
//          : base("Merged Shader Render Item", "MS Ri", "Merged Shader Render Item For OutLine")
//        {
//        }

//        #region Calculate
//        /// <summary>
//        /// Registers all the input parameters for this component.
//        /// </summary>
//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddParameter(new RenderItemParameter(GH_ParamAccess.list));
//            pManager.AddParameter(new CurveRenderAttributeParameter(), "Intersect Line Attribute", "IA", "Intersect Line Attribute", GH_ParamAccess.item);
//            pManager[1].Optional = true;
//            pManager.AddParameter(new CurveRenderAttributeParameter(), "Out Line Attribute", "OA", "Out Line Attribute", GH_ParamAccess.item);
//            pManager[2].Optional = true;
//        }

//        /// <summary>
//        /// This is the method that actually does the work.
//        /// </summary>
//        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            List<MeshRenderItem> meshRenderItems = new List<MeshRenderItem>();
//            CurveRenderAttribute interAtt = null;
//            CurveRenderAttribute outAtt = null;

//            DA.GetDataList(0, meshRenderItems);
//            DA.GetData(1, ref interAtt);
//            DA.GetData(2, ref outAtt);

//            DA.SetData(0, new MergedMeshRenderItem(meshRenderItems, outAtt, interAtt));
//        }
//        #endregion
//    }
//}