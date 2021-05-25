/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArchiVision
{
    public abstract class BaseComponent : GH_Component
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.hidden;


        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("91a18e4a-efda-4f1d-a1e2-beb0440639ba");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the BaseComponent class.
        /// </summary>
        public BaseComponent(string name, string nickname, string description, Subcategory subcategory)
          : base(name, nickname, description, "ArchiVision", subcategory.ToString())
        {
        }

        protected void AddSection(GH_Component.GH_InputParamManager pManager)
        {
            Param_GenericObject obj = new Param_GenericObject();
            obj.Optional = true;
            pManager.AddParameter(obj, "------", "---", "------", GH_ParamAccess.tree);
        }
    }
}