/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    /// <summary>
    /// Make display item have some attributes.
    /// </summary>
    public abstract class AttributeDisplayItem : DisplayItem
    {
        public virtual Color Colour { get; protected set; }
        public IGH_PreviewData Geometry { get; protected set; }

        public override BoundingBox ClippingBox
        {
            get
            {
                BoundingBox box = Geometry.ClippingBox;
                box.Union(base.ClippingBox);
                return box;
            }
        }

        public AttributeDisplayItem(IGH_PreviewData geometry, Color color = default(Color), bool topMost = false)
            :base(topMost)
        {
            Geometry = geometry;
            Colour = color;
        }
    }
}
