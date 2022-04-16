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
    public class WindowParameter : GH_Param<WindowGoo>
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.WindowParameter_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("bf049c0c-164f-452f-b0a1-8133e574be0b");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the WindowParameter class.
        /// </summary>
        public WindowParameter(string name, string nickname, string description, string category, string subcategory, GH_ParamAccess access)
            : base(name, nickname, description, category, subcategory, access)
        {
        }

        public WindowParameter()
            : base("Window", "W", "Window", "ArchiVision", "UI Element", GH_ParamAccess.item)
        {
        }

    }
}