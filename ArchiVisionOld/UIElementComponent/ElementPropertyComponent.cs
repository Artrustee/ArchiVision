/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using MaterialDesignThemes.Wpf;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace ArchiVision.UIElementComponent
{
    public class ElementPropertyComponent : ElementComponent
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.ElementPropertyComponent_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("9ce49a34-020c-4b08-8a79-12cb1826e310");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the ElementPropertyComponent class.
        /// </summary>
        public ElementPropertyComponent()
          : base("Set Element Property", "Ele Prop",
              "Set Element Property", Subcategory.UI_Element)
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new UIElementParameter());
            pManager.AddNumberParameter("Width", "W", "Width", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Height", "H", "Height", GH_ParamAccess.item);
            pManager[2].Optional = true;

            pManager.AddIntegerParameter(nameof(HorizontalAlignment), "HL", nameof(HorizontalAlignment), GH_ParamAccess.item);
            pManager[3].Optional = true;
            Param_Integer param1 = (Param_Integer)pManager[3];
            param1.AddNamedValue(nameof(HorizontalAlignment.Left), (int)HorizontalAlignment.Left);
            param1.AddNamedValue(nameof(HorizontalAlignment.Center), (int)HorizontalAlignment.Center);
            param1.AddNamedValue(nameof(HorizontalAlignment.Right), (int)HorizontalAlignment.Right);
            param1.AddNamedValue(nameof(HorizontalAlignment.Stretch), (int)HorizontalAlignment.Stretch);

            pManager.AddIntegerParameter(nameof(VerticalAlignment), "VA", nameof(VerticalAlignment), GH_ParamAccess.item);
            pManager[4].Optional = true;
            Param_Integer param2 = (Param_Integer)pManager[4];
            param2.AddNamedValue(nameof(VerticalAlignment.Top), (int)VerticalAlignment.Top);
            param2.AddNamedValue(nameof(VerticalAlignment.Center), (int)VerticalAlignment.Center);
            param2.AddNamedValue(nameof(VerticalAlignment.Bottom), (int)VerticalAlignment.Bottom);
            param2.AddNamedValue(nameof(VerticalAlignment.Stretch), (int)VerticalAlignment.Stretch);

            AddSection(pManager);

            pManager.AddNumberParameter("Margin", "M", "Margin with four numbers(left, top ,right, bottom), or one.", GH_ParamAccess.list);
            pManager[6].Optional = true;

            pManager.AddNumberParameter("Padding", "P", "Padding with four numbers(left, top ,right, bottom), or one.", GH_ParamAccess.list);
            pManager[7].Optional = true;

            pManager.AddIntegerParameter(nameof(ShadowDepth), "SD", nameof(ShadowDepth), GH_ParamAccess.item);
            pManager[8].Optional = true;
            Param_Integer param3 = (Param_Integer)pManager[8];
            param3.AddNamedValue(nameof(ShadowDepth.Depth0), (int)ShadowDepth.Depth0);
            param3.AddNamedValue(nameof(ShadowDepth.Depth1), (int)ShadowDepth.Depth1);
            param3.AddNamedValue(nameof(ShadowDepth.Depth2), (int)ShadowDepth.Depth2);
            param3.AddNamedValue(nameof(ShadowDepth.Depth3), (int)ShadowDepth.Depth3);
            param3.AddNamedValue(nameof(ShadowDepth.Depth4), (int)ShadowDepth.Depth4);
            param3.AddNamedValue(nameof(ShadowDepth.Depth5), (int)ShadowDepth.Depth5);


        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            UIElement element = null;
            DA.GetData(0, ref element);

            if (!(element is FrameworkElement)) return;
            FrameworkElement frameEle = (FrameworkElement)element;

            double dou = 0;
            if (DA.GetData(1, ref dou)) frameEle.Width = dou;
            if (DA.GetData(2, ref dou)) frameEle.Height = dou;

            int inte = 0;
            if (DA.GetData(3, ref inte)) frameEle.HorizontalAlignment = (HorizontalAlignment)inte;
            if (DA.GetData(4, ref inte)) frameEle.VerticalAlignment = (VerticalAlignment)inte;

            List<double> values = new List<double>();
            if(DA.GetDataList(6, values)) 
                if(values.Count == 1)
                {
                    frameEle.Margin = new Thickness(values[0]);
                }
                else if (values.Count > 1)
                {
                    frameEle.Margin = new Thickness(values[0], values[1], values[2 % values.Count], values[3 % values.Count]);
                }

            if (DA.GetDataList(7, values) && frameEle is Control)
            {
                if (values.Count == 1)
                {
                    ((Control)frameEle).Padding = new Thickness(values[0]);
                }
                else if (values.Count > 1)
                {
                    ((Control)frameEle).Padding = new Thickness(values[0], values[1], values[2 % values.Count], values[3 % values.Count]);
                }
            }


            if (DA.GetData(8, ref inte)) ShadowAssist.SetShadowDepth(frameEle, (ShadowDepth)inte);

            DA.SetData(0, element);
        }
        #endregion
    }
}