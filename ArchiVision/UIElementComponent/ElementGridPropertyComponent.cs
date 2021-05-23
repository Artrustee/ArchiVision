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

namespace ArchiVision.UIElementComponent
{
    public class ElementGridPropertyComponent : ElementComponent
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.ElementGridPropertyComponent_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("c05a422f-ca1e-400a-82b1-0003ae49f9e0");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the ElementGridPropertyComponent class.
        /// </summary>
        public ElementGridPropertyComponent()
          : base("ElementGridPropertyComponent", "Nickname",
              "Description", nameof(Subcategory.UI_Container))
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new UIElementParameter());

            pManager.AddIntegerParameter("RowIndex", "RI", "RowIndex", GH_ParamAccess.item);
            pManager[1].Optional = true;

            pManager.AddIntegerParameter("ColumnIndex", "CI", "ColumnIndex", GH_ParamAccess.item);
            pManager[2].Optional = true;

            pManager.AddIntegerParameter("RowSpan", "RS", "RowSpan", GH_ParamAccess.item);
            pManager[3].Optional = true;

            pManager.AddIntegerParameter("ColumnSpan", "CS", "ColumnSpan", GH_ParamAccess.item);
            pManager[4].Optional = true;
        }


        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            UIElement element = null;
            DA.GetData(0, ref element);
            if (element == null) return;

            int inte = 0;
            if (DA.GetData(1, ref inte)) Grid.SetRow(element, inte);
            if (DA.GetData(2, ref inte)) Grid.SetColumn(element, inte);
            if (DA.GetData(3, ref inte)) Grid.SetRowSpan(element, inte);
            if (DA.GetData(4, ref inte)) Grid.SetColumnSpan(element, inte);

            DA.SetData(0, element);
        }
        #endregion
    }
}