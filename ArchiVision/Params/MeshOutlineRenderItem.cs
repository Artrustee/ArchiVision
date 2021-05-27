/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel.Types;
using Rhino;
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
    public class MeshOutlineRenderItem : CurveRenderItem
    {
        public Linetype LineType { get; set; }

        public MeshOutlineRenderItem(GH_Mesh mesh, CurveRenderAttribute att)
            : base(null, att, true)
        {
            Geometry = mesh;
            LineType = att.LineType;
        }

        public override BoundingBox ClippingBox
        {
            get
            {
                BoundingBox box = ((GH_Mesh)Geometry).Value.Offset(5 * InputSize).GetBoundingBox(false);
                box.Union(base.ClippingBox);
                return box;
            }
        }

        public override void DrawViewportWires(RhinoViewport Viewport, DisplayPipeline Display, Rectangle3d drawRect, double unitPerPx, Color WireColour_Selected, DisplayMaterial ShadeMaterial_Selected, bool selected)
        {
            PatternCurve.Clear();
            Points.Clear();

            double thickness = GetSize(Viewport, unitPerPx);
            double tolerance = RhinoDoc.ActiveDoc?.ModelAbsoluteTolerance ?? 0.001;
            //double offsetDistance = thickness / unitPerPx / 2;
            double offsetDistance = 0;
            ((GH_Mesh)Geometry).Value.GetOutlines(new ViewportInfo(Viewport), drawRect.Plane).ToList().ForEach((polyline) =>
            {
                //Polyline poly = CurveExtend(polyline, 5 * (thickness + offsetDistance));

                Curve poly = polyline.ToPolylineCurve();
                if (!polyline.IsClosed)
                    poly = poly.Extend(CurveEnd.Both, (thickness + offsetDistance), CurveExtensionStyle.Line);

                var crvs = poly.Offset(drawRect.Plane, offsetDistance, tolerance, CurveOffsetCornerStyle.Sharp);
                if(crvs == null)
                {
                    CreatePatternCurve(new GH_Curve(poly), LineType);
                }
                else crvs.ToList().ForEach((c) =>
                {
                    CreatePatternCurve(new GH_Curve(c), LineType);
                });
            });
            base.DrawViewportWires(Viewport, Display, drawRect, unitPerPx, WireColour_Selected, ShadeMaterial_Selected, selected);
        }

        public static Polyline CurveExtend(Polyline curve, double length)
        {
            if (curve == null || curve.IsClosed || curve.Count < 2) return curve;

            Vector3d startDir = Point3d.Subtract(curve[0], curve[1]);
            Vector3d endDir = Point3d.Subtract(curve[curve.Count - 1], curve[curve.Count - 2]);

            startDir.Unitize();
            startDir *= length;
            endDir.Unitize();
            endDir *= length;

            Point3d startPt = Point3d.Add(curve[0], startDir);
            Point3d endPt = Point3d.Add(curve[curve.Count - 1], endDir);

            return ReplaceBothPoint(curve, startPt, endPt);
        }

        private static Polyline ReplaceBothPoint(Polyline curve, Point3d startPt, Point3d endPt)
        {
           return  ReplacePoint(ReplacePoint(curve, 0, startPt), curve.Count - 1, endPt);
        }

        private static Polyline ReplacePoint(Polyline curve, int index, Point3d newPt)
        {
            curve.RemoveAt(index);
            curve.Insert(index, newPt);
            return curve;
        }
    }
}
