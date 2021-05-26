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
    public class ArrowCurveRenderItem : CurveRenderItem
    {

        public bool StartArrow { get; protected set; }
        public bool EndArrow { get; protected set; }

        public double ArrowMult { get; protected set; }

        public ArrowCurveRenderItem(GH_Curve curve, CurveRenderAttribute att, bool start, bool end, double mult)
            :base(curve, att)
        {
            StartArrow = start;
            EndArrow = end;
            ArrowMult = mult;
        }

        public ArrowCurveRenderItem(GH_Curve curve, Color color, double thickness, Linetype linetype, bool start, bool end, double mult, bool absolute)
            : base(curve, color, thickness, linetype, absolute)
        {
            StartArrow = start;
            EndArrow = end;
            ArrowMult = mult;
        }

        public override void DrawViewportWires(RhinoViewport Viewport, DisplayPipeline Display, Color WireColour_Selected, DisplayMaterial ShadeMaterial_Selected, bool selected)
        {
            base.DrawViewportWires(Viewport, Display, WireColour_Selected, ShadeMaterial_Selected, selected);

            Color colour = selected ? WireColour_Selected : Colour;
            double vpSize = GetSize(Viewport);

            Curve curve = ((GH_Curve)Geometry).Value;
            double arrowSize = vpSize * ArrowMult;

            if (StartArrow)
                Display.DrawArrowHead(curve.PointAtStart, new Vector3d(-curve.TangentAtStart.X, -curve.TangentAtStart.Y, -curve.TangentAtStart.Z), colour, arrowSize, arrowSize);
            if (EndArrow)
                Display.DrawArrowHead(curve.PointAtEnd, curve.TangentAtEnd, colour, arrowSize, arrowSize);
        }
    }
}