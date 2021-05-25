/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Display;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    public class PointRenderItem : BaseRenderItem
    {
        public PointStyle PtStyle { get; protected set; }
        public float Radius { get; protected set; }

        public PointRenderItem(GH_Point point, Color color, PointStyle style, float radius)
            :base(point, color)
        {
            PtStyle = style;
            Radius = radius;
        }

        public override void DrawViewportWires(IGH_PreviewArgs args, bool selected)
        {
            args.Display.DrawPoint(((GH_Point)Geometry).Value, PtStyle, Radius, Colour);
        }
    }
}
