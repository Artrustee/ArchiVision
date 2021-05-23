/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms;

namespace ArchiVision.UIElementComponent
{
    public class SliderElementComponent : ImportElementComponent<GH_NumberSlider>
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.SliderElementComponent_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("01a6648e-fa96-479e-a738-8c2e243ac34f");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the SliderElementComponent class.
        /// </summary>
        public SliderElementComponent()
          : base("Slider UI Element", "Slider","Slider UI Element")
        {
        }


        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            List<Grid> grids = new List<Grid>();
            foreach (var slider in DocObjects)
            {
                grids.Add(UserControlHelper.NumberSlider(slider));
            }
            DA.SetDataList(0, grids);
        }
        #endregion
    }
}