/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace ArchiVision
{
    public class WindowUIComponent : GH_Component
    {
        #region Values
        public MaterialDesignWindow MDWindow { get;protected set; } = new MaterialDesignWindow();
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.WindowComponent_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("3c2aa3ca-c2e4-4b2b-9af9-34104f190387");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the WindowComponent class.
        /// </summary>
        public WindowUIComponent()
          : base("Window", "Win",
              "Window",
              "ArchiVision", nameof(Subcategory.UI_Window))
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Show", "S", "Show", GH_ParamAccess.item, false);

            pManager.AddNumberParameter("Width", "W", "Width", GH_ParamAccess.item, 800);
            pManager.AddNumberParameter("Height", "H", "Height", GH_ParamAccess.item, 450);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new WindowParameter());
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool run = false;
            double width = 800;
            double height = 450;

            DA.GetData(0, ref run);
            DA.GetData(1, ref width);
            DA.GetData(2, ref height);

            MDWindow.Width = width;
            MDWindow.Height = height;

            if (run) 
            {
                try
                {
                    MDWindow.Show();
                }
                catch
                {
                    MDWindow = new MaterialDesignWindow();
                    this.ExpireSolution(true);
                }
            }
            else
            {
                MDWindow.Hide();
            }
            DA.SetData(0, MDWindow);
        }
        #endregion
    }
}