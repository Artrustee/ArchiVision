/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArchiVision
{
    public class CurveRenderItem : BaseRenderItem
    {
        public int Thickness { get; set; }
        public CurveRenderItem(GH_Curve curve, Color color, int thickness)
            : base(curve, color)
        {
        }

        public override void DrawViewportWires(IGH_PreviewArgs args, bool selected)
        {
            args.Display.DrawCurve(((GH_Curve)Geometry).Value, selected? args.WireColour_Selected:Colour, Thickness);
        }
    }
}