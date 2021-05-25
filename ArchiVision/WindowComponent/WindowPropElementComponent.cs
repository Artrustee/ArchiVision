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
using System.Windows.Media;

namespace ArchiVision.WindowComponent
{
    public class WindowPropElementComponent : BaseComponent
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.WindowAddElementComponent_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("6d3bdfa1-f7a3-4731-adbc-3cea473ebad1");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the WindowAddElementComponent class.
        /// </summary>
        public WindowPropElementComponent()
          : base("Window Property", "Wind Prop",
              "Window Property",Subcategory.UI_Window)
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new WindowParameter());
            pManager.AddParameter(new UIElementParameter());

            AddSection(pManager);

            pManager.AddColourParameter("PrimaryHueLightBrush", "PL", "PrimaryHueLightBrush", GH_ParamAccess.item);
            pManager[3].Optional = true;

            pManager.AddColourParameter("PrimaryHueLightForegroundBrush", "PLF", "PrimaryHueLightForegroundBrush", GH_ParamAccess.item);
            pManager[4].Optional = true;

            pManager.AddColourParameter("PrimaryHueMidBrush", "PM", "PrimaryHueMidBrush", GH_ParamAccess.item);
            pManager[5].Optional = true;

            pManager.AddColourParameter("PrimaryHueMidForegroundBrush", "PMF", "PrimaryHueMidForegroundBrush", GH_ParamAccess.item);
            pManager[6].Optional = true;

            pManager.AddColourParameter("PrimaryHueDarkBrush", "PD", "PrimaryHueDarkBrush", GH_ParamAccess.item);
            pManager[7].Optional = true;

            pManager.AddColourParameter("PrimaryHueDarkForegroundBrush", "PDF", "PrimaryHueDarkForegroundBrush", GH_ParamAccess.item);
            pManager[8].Optional = true;

            AddSection(pManager);

            pManager.AddColourParameter("SecondaryHueMidBrush", "SM", "SecondaryHueMidBrush", GH_ParamAccess.item);
            pManager[10].Optional = true;

            pManager.AddColourParameter("SecondaryHueMidForegroundBrush", "SM", "SecondaryHueMidForegroundBrush", GH_ParamAccess.item);
            pManager[11].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            MaterialDesignWindow window = null;
            UIElement ele = null;
            DA.GetData(0, ref window);
            DA.GetData(1, ref ele);

            Helper.RemoveParent(ele);
            window.Content = ele;

            System.Drawing.Color relay = System.Drawing.Color.Empty;
           
            if (DA.GetData(3, ref relay))
            {
                window.Resources.MergedDictionaries[0].Remove("PrimaryHueLightBrush");
                window.Resources.MergedDictionaries[0].Add("PrimaryHueLightBrush", new SolidColorBrush(System.Windows.Media.Color.FromArgb(relay.A, relay.R, relay.G, relay.B)));
            }
            if (DA.GetData(4, ref relay))
            {
                window.Resources.MergedDictionaries[0].Remove("PrimaryHueLightForegroundBrush");
                window.Resources.MergedDictionaries[0].Add("PrimaryHueLightForegroundBrush", new SolidColorBrush(System.Windows.Media.Color.FromArgb(relay.A, relay.R, relay.G, relay.B)));
            }

            if (DA.GetData(5, ref relay))
            {
                window.Resources.MergedDictionaries[0].Remove("PrimaryHueMidBrush");
                window.Resources.MergedDictionaries[0].Add("PrimaryHueMidBrush", new SolidColorBrush(System.Windows.Media.Color.FromArgb(relay.A, relay.R, relay.G, relay.B)));
            }
            if (DA.GetData(6, ref relay))
            {
                window.Resources.MergedDictionaries[0].Remove("PrimaryHueMidForegroundBrush");
                window.Resources.MergedDictionaries[0].Add("PrimaryHueMidForegroundBrush", new SolidColorBrush(System.Windows.Media.Color.FromArgb(relay.A, relay.R, relay.G, relay.B)));
            }

            if (DA.GetData(7, ref relay))
            {
                window.Resources.MergedDictionaries[0].Remove("PrimaryHueDarkBrush");
                window.Resources.MergedDictionaries[0].Add("PrimaryHueDarkBrush", new SolidColorBrush(System.Windows.Media.Color.FromArgb(relay.A, relay.R, relay.G, relay.B)));
            }
            if (DA.GetData(8, ref relay))
            {
                window.Resources.MergedDictionaries[0].Remove("PrimaryHueDarkForegroundBrush");
                window.Resources.MergedDictionaries[0].Add("PrimaryHueDarkForegroundBrush", new SolidColorBrush(System.Windows.Media.Color.FromArgb(relay.A, relay.R, relay.G, relay.B)));
            }

            if (DA.GetData(10, ref relay))
            {
                window.Resources.MergedDictionaries[1].Remove("SecondaryHueMidBrush");
                window.Resources.MergedDictionaries[1].Add("SecondaryHueMidBrush", new SolidColorBrush(System.Windows.Media.Color.FromArgb(relay.A, relay.R, relay.G, relay.B)));
            }
            if (DA.GetData(11, ref relay))
            {
                window.Resources.MergedDictionaries[1].Remove("SecondaryHueMidForegroundBrush");
                window.Resources.MergedDictionaries[1].Add("SecondaryHueMidForegroundBrush", new SolidColorBrush(System.Windows.Media.Color.FromArgb(relay.A, relay.R, relay.G, relay.B)));
            }

            window.UpdateLayout();
        }
        #endregion
    }
}