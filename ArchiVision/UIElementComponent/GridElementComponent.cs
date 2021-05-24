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
using System.Windows.Media;

namespace ArchiVision.UIElementComponent
{
    public class GridElementComponent : ElementComponent    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.GridElementComponent_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("198e6ee9-5097-4768-a2f9-255bdc0f51ef");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the GridComponent class.
        /// </summary>
        public GridElementComponent()
          : base("Grid Element", "Grid",
              "Grid Element", Subcategory.UI_Container)
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new UIElementParameter(), "UI Elements", "Es", "UI Elements", GH_ParamAccess.list);
            pManager.AddTextParameter("Row Definitions", "RD", "An optional list of Row Heights. Use numbers for absolute sizes and numbers with * for ratios (like 1* and 2* for a 1/3 2/3 split)", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddTextParameter("Column Definitions", "CD", "An optional list of Column Widths. Use numbers for absolute sizes and numbers with * for ratios (like 1* and 2* for a 1/3 2/3 split)", GH_ParamAccess.list);
            pManager[2].Optional = true;
        }


        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<UIElement> eles = new List<UIElement>();
            List<string> rowDef = new List<string>();
            List<string> colDef = new List<string>();

            if (!DA.GetDataList(0, eles)) return;

            Grid grid = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            GridLengthConverter gridLengthConverter = new GridLengthConverter();

            if (DA.GetDataList(1, rowDef))
            {
                foreach (string rowitem in rowDef)
                {
                    RowDefinition rowDefinition = new RowDefinition();
                    rowDefinition.Height = (GridLength)gridLengthConverter.ConvertFromString(rowitem);
                    grid.RowDefinitions.Add(rowDefinition);
                }
            }
            if (DA.GetDataList(2, colDef))
            {
                foreach (string colitem in colDef)
                {
                    ColumnDefinition columnDefinition = new ColumnDefinition();
                    columnDefinition.Width = (GridLength)gridLengthConverter.ConvertFromString(colitem);
                    grid.ColumnDefinitions.Add(columnDefinition);
                }
            }

            eles.ForEach((ele) =>
            {
                Helper.RemoveParent(ele);
                grid.Children.Add(ele);
            });


            DA.SetData(0, grid);


        }
        #endregion
    }
}