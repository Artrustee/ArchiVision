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

namespace ArchiVision
{
    public abstract class ElementComponent: GH_Component 
    {
        #region Values


        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("dae0a03e-e594-4c79-baab-f02f1214a915");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the ElementComponent class.
        /// </summary>
        public ElementComponent(string name, string nickName, string description, string subCate)
          : base(name, nickName, description, "ArchiVision", subCate)
        {
        }

        #region Calculate

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new UIElementParameter());
        }
        #endregion
    }
}