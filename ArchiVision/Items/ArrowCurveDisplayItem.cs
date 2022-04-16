/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArchiVision
{
    public class ArrowCurveDisplayItem : CurveDisplayItem
    {

        public bool StartArrow { get; protected set; }
        public bool EndArrow { get; protected set; }

        public double ArrowMult { get; protected set; }

        public ArrowCurveDisplayItem(GH_Curve curve, CurveDisplayAttribute att, bool start, bool end, double mult)
            :base(curve, att)
        {
            StartArrow = start;
            EndArrow = end;
            ArrowMult = mult;
        }

        public ArrowCurveDisplayItem(GH_Curve curve, Color color, double thickness, Linetype linetype, bool start, bool end, double mult, bool absolute)
            : base(curve, color, thickness, linetype, absolute)
        {
            StartArrow = start;
            EndArrow = end;
            ArrowMult = mult;
        }

        public override void DrawViewportWires(RhinoViewport Viewport, DisplayPipeline Display, Rectangle3d drawRect, double unitPerPx, Color WireColour_Selected, DisplayMaterial ShadeMaterial_Selected, bool selected)
        {
            base.DrawViewportWires(Viewport, Display, drawRect, unitPerPx, WireColour_Selected, ShadeMaterial_Selected, selected);

            if (!IsTopMost) return;

            Color colour = selected ? WireColour_Selected : Colour;
            double vpSize = GetSize(Viewport, unitPerPx);

            Curve curve = ((GH_Curve)Geometry).Value;
            double arrowSize = vpSize * ArrowMult;

            if (StartArrow)
                Display.DrawArrowHead(curve.PointAtStart - curve.TangentAtStart * arrowSize, -curve.TangentAtStart, colour, arrowSize, arrowSize);
            if (EndArrow)
                Display.DrawArrowHead(curve.PointAtEnd , curve.TangentAtEnd, colour, arrowSize, arrowSize);
        }
    }
}