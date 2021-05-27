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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    public class CurveRenderItem : SizableRenderItem
    {
        public List<Curve> PatternCurve { get; } = new List<Curve>();

        public List<Point3d> Points { get; } = new List<Point3d>();

        public CurveRenderItem(GH_Curve curve, CurveRenderAttribute att, bool topMost = false)
            : this(curve, att.Colour, att.Thickness, att.LineType, att.Absolute, topMost)
        {
        }

        public CurveRenderItem(GH_Curve curve, Color color, double thickness, Linetype linetype, bool absolute, bool topMost = false)
            : base(curve, thickness, absolute, color, topMost)
        {
            PatternCurve.Clear();
            Points.Clear();

            if (curve != null)
                CreatePatternCurve(curve, linetype);
        }

        protected void CreatePatternCurve(GH_Curve curve, Linetype linetype)
        {
            if (linetype.Index == -1)
            {
                PatternCurve.Add(curve.Value);
                return;
            }

            double scale = Rhino.RhinoDoc.ActiveDoc.Linetypes.LinetypeScale;

            List<double> lengths = new List<double>();
            double totalLen = 0;
            for (int i = 0; i < linetype.SegmentCount; i++)
            {
                double length;
                bool solid;
                linetype.GetSegment(i, out length, out solid);

                double realLength = length * scale;
                totalLen += realLength;
                lengths.Add(realLength);
            }

            foreach (Curve crvDiv in CurvePeriod(curve.Value, totalLen))
            {
                PartternCurve(crvDiv, lengths);
            }
        }
        private void PartternCurve(Curve curve, List<double> pattern)
        {
            for (int i = 0; i < pattern.Count; i++)
            {
                double length = pattern[i];
                if (length == 0)
                {
                    Points.Add(new Point3d(curve.PointAtStart));
                    continue;
                }
                if (curve.GetLength() <= length)
                {
                    if (i % 2 == 0) PatternCurve.Add(curve);
                    continue;
                }

                double t;
                curve.LengthParameter(length, out t);
                Curve[] crvs = curve.Split(t);
                if (crvs == null) continue;

                curve = crvs[1];
                if (i % 2 == 0) PatternCurve.Add(crvs[0]);
            }
        }

        private Curve[] CurvePeriod(Curve curve, double length)
        {
            List<Curve> curves = new List<Curve>();
            bool goon = true;
            while (goon)
            {
                double t;
                goon = curve.LengthParameter(length, out t);
                if (goon)
                {
                    Curve[] crvs = curve.Split(t);
                    if (crvs == null) break;
                    curves.Add(crvs[0]);
                    curve = crvs[1];
                }
            }
            curves.Add(curve);

            return curves.ToArray();
        }

        public override void DrawViewportWires(RhinoViewport Viewport, DisplayPipeline Display, Rectangle3d drawRect, double unitPerPx, Color WireColour_Selected, DisplayMaterial ShadeMaterial_Selected, bool selected)
        {
            Color colour = selected ? WireColour_Selected : Colour;
            double vpSize = GetSize(Viewport, unitPerPx);

            PatternCurve.ForEach((crv) => Display.DrawCurve(crv, colour, (int)(vpSize)));
            Display.DrawPoints(Points, PointStyle.Circle, (int)(vpSize / 2), colour);
        }
    }
}
