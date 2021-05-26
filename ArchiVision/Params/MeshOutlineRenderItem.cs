/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

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
    public class MeshOutlineRenderItem : CurveRenderItem
    {
        public Linetype LineType { get; set; }

        public MeshOutlineRenderItem(GH_Mesh mesh, CurveRenderAttribute att)
            : base(null, att, true)
        {
            Geometry = mesh;
            LineType = att.LineType;
        }

        public override void DrawViewportWires(RhinoViewport Viewport, DisplayPipeline Display, Color WireColour_Selected, DisplayMaterial ShadeMaterial_Selected, bool selected)
        {
            PatternCurve.Clear();
            Points.Clear();

            // get the current view corners
            Point3d[] m_farCorners = Viewport.GetFarRect();
            Point3d[] m_nearCorners = Viewport.GetNearRect();

            Point3d[] m_viewCorners = new Point3d[3]
            {
                (m_farCorners[0] * 0.15 + m_nearCorners[0] * 0.85),
                (m_farCorners[1] * 0.15 + m_nearCorners[1] * 0.85),
                (m_farCorners[2] * 0.15 + m_nearCorners[2] * 0.85)
                };

            //e.Viewport.GetNearRect();

            Plane m_drawPlane = new Plane(m_viewCorners[0], m_viewCorners[1], m_viewCorners[2]);

            double m_viewWidth = m_viewCorners[0].DistanceTo(m_viewCorners[1]);
            double m_unitPerPx = m_viewWidth / Viewport.Size.Width;
            Curve.JoinCurves(((GH_Mesh)Geometry).Value.GetOutlines(new ViewportInfo(Viewport), m_drawPlane).Select((polyline) => polyline.ToNurbsCurve())).ToList().ForEach((polyline) =>
            {
                Curve curve = polyline.ToNurbsCurve();
                Curve crv = curve.Extend(CurveEnd.Both, 5 * GetSize(Viewport), CurveExtensionStyle.Line);
                crv = crv ?? curve;
                CreatePatternCurve(new GH_Curve(crv), LineType);
            });

            base.DrawViewportWires(Viewport, Display, WireColour_Selected, ShadeMaterial_Selected, selected);
        }

        protected override double GetSizeMulty(RhinoViewport viewport)
        {
            // get the current view corners
            Point3d[] m_farCorners = viewport.GetFarRect();
            Point3d[] m_nearCorners = viewport.GetNearRect();

            Point3d[] m_viewCorners = new Point3d[3]
            {
                (m_farCorners[0] * 0.15 + m_nearCorners[0] * 0.85),
                (m_farCorners[1] * 0.15 + m_nearCorners[1] * 0.85),
                (m_farCorners[2] * 0.15 + m_nearCorners[2] * 0.85)
                };

            //e.Viewport.GetNearRect();

            double m_viewWidth = m_viewCorners[0].DistanceTo(m_viewCorners[1]);

            double vpSize = 1;
            if (!Absolute) viewport.GetWorldToScreenScale(Geometry.ClippingBox.Center, out vpSize);

            return   viewport.Size.Width/m_viewWidth / vpSize;
        }
    }
}
