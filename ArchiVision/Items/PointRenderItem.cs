/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    public class PointRenderItem : SizableDisplayIItem
    {
        public PointStyle PtStyle { get; protected set; }

        public PointRenderItem(GH_Point point, Color color, PointStyle style, float radius, bool absolute)
            :base(point, radius, absolute, color)
        {
            PtStyle = style;
        }
        public override void DrawViewportWires(RhinoViewport Viewport, DisplayPipeline Display, Rectangle3d drawRect, double unitPerPx, Color WireColour_Selected, DisplayMaterial ShadeMaterial_Selected, bool selected)
        {
            Display.DrawPoint(((GH_Point)Geometry).Value, PtStyle, (int)GetSize(Viewport, unitPerPx), Colour);
        }
    }
}
