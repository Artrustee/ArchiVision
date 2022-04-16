/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArchiVision
{
    public class UIElementParameter : GH_Param<UIElementGoo>
    {
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.UIElementParameter_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("639afc51-e9ae-4e60-b002-9c2ec187c499");


        #endregion

        /// <summary>
        /// Initializes a new instance of the UIElementParameter class.
        /// </summary>
        public UIElementParameter(string name, string nickname, string description, string category, string subcategory, GH_ParamAccess access)
                : base(name, nickname, description, category, subcategory, access)
        {
        }

        public UIElementParameter()
        : base("UI Element", "E", "UI Element", "ArchiVision", Subcategory.UI_Element.ToString(), GH_ParamAccess.item)
        {
        }
    }
}