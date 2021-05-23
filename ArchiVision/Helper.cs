/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ArchiVision
{
    public static class Helper
    {
        private static Type _viewType = null;

        private static Type FindViewParam()
        {
            //CurveComponents.Make2DViewParameter
            //7069208c-c471-4b82-bae6-e938f16dacb0

            foreach (IGH_ObjectProxy proxy in Grasshopper.Instances.ComponentServer.ObjectProxies)
            {
                if (proxy.Guid != new Guid("3fc08088-d75d-43bc-83cc-7a654f156cb7")) continue;
                return ((GH_Component)proxy.CreateInstance()).Params.Output[0].GetType();
            }
            return null;
        }

        public static IGH_Param CreateViewParam()
        {
            _viewType = _viewType ?? FindViewParam();
            if (_viewType == null) throw new ArgumentNullException();

            return (IGH_Param)Activator.CreateInstance(_viewType, "View", "V", "View", "Display", "Dimensions", GH_ParamAccess.item);
        }

        public static void RemoveParent(UIElement ele)
        {
            var parent = VisualTreeHelper.GetParent(ele);
            if (parent == null) return;

            if (parent is ContentControl)
                (parent as ContentControl).Content = null;
            else if (parent is Panel)
                (parent as Panel).Children.Remove(ele);
        }
    }
}
