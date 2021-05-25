/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    public abstract class BaseRenderItem
    {
        public virtual Color Colour { get; protected set; }
        public IGH_PreviewData Geometry { get; protected set; }

        public BaseRenderItem(IGH_PreviewData geometry, Color color = default(Color))
        {
            Geometry = geometry;
            Colour = color;
        }

        public virtual void DrawViewportWires(IGH_PreviewArgs args, bool selected) { }

        public virtual void DrawViewportMeshes(IGH_PreviewArgs args, bool selected) { }
    }
}
