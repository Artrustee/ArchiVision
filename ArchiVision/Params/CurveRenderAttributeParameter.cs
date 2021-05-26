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
    public class CurveRenderAttributeParameter : GH_Param<CurveRenderAttributeGoo>
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.CurveRenderAttributeParameter_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("99e97220-998e-4f76-8f9f-1ccc1bb0b98e");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the CurveRenderAttributeParameter class.
        /// </summary>
        /// <summary>
        /// Initializes a new instance of the UIElementParameter class.
        /// </summary>
        public CurveRenderAttributeParameter(string name, string nickname, string description, string category, string subcategory, GH_ParamAccess access)
                : base(name, nickname, description, category, subcategory, access)
        {
        }

        public CurveRenderAttributeParameter()
        : base("Curve Render Attribute", "CA", "Curve Render Attribute", "ArchiVision", Subcategory.UI_Element.ToString(), GH_ParamAccess.item)
        {
        }
    }
}