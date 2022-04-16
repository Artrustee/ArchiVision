/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{

    public class ArchiVisionConduit : DisplayConduit
    {
        public List<RenderItemParameter> ItemParams { get; } = new List<RenderItemParameter>();
        public List<RhinoViewPropertyComponent> PropertyComponents { get; } = new List<RhinoViewPropertyComponent>();

        public Rectangle3d DrawRect { get; protected set; }

        public double UnitPerPx { get; protected set; }

        protected override void CalculateBoundingBox(CalculateBoundingBoxEventArgs e)
        {
            base.CalculateBoundingBox(e);

            CaBoundingBox(e, FindRenderItems(e.Viewport));
        }


        protected override void DrawForeground(DrawEventArgs e)
        {
            base.DrawForeground(e);
            DrawFore(e, FindRenderItems(e.Viewport));
        }

        public List<BaseRenderItem> FindRenderItems(RhinoViewport viewport)
        {
            List<BaseRenderItem> result = new List<BaseRenderItem>();
            foreach (var item in PropertyComponents)
            {
                var lt = item.FindRenderItems(viewport);
                if (lt == null) continue;
                result.AddRange(lt);
            }
            foreach (var item in ItemParams)
            {
                //result.AddRange(item.FindItems());
            }
            return result;
        }

        protected void CaBoundingBox(CalculateBoundingBoxEventArgs e, List<BaseRenderItem> _items)
        {
            UpdateDrawInfo(e.Viewport);
            BoundingBox box = BoundingBox.Empty;
            DrawRect.ToNurbsCurve().Offset(DrawRect.Plane, Math.Max(DrawRect.Width, DrawRect.Height) / 4, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance, CurveOffsetCornerStyle.Sharp).ToList().ForEach((curve) =>
            {
                box.Union(curve.GetBoundingBox(false));
            });

            e.IncludeBoundingBox(box);

            if (_items == null) return;

            BoundingBox b = BoundingBox.Empty;
            _items.ForEach((item) => b.Union(item.ClippingBox));

            e.IncludeBoundingBox(b);
        }

        protected void DrawFore(DrawEventArgs e, List<BaseRenderItem> _items)
        {
            if (_items == null) return;
            _items.ForEach((item) =>
            {
                item.DrawViewportWires(e, DrawRect, UnitPerPx);
                item.DrawViewportMeshes(e, DrawRect, UnitPerPx);
            });
        }

        public void UpdateDrawInfo(RhinoViewport viewport)
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
            Plane m_drawPlane = new Plane(m_viewCorners[0], m_viewCorners[1], m_viewCorners[2]);
            double m_viewWidth = m_viewCorners[0].DistanceTo(m_viewCorners[1]);
            double m_viewHeight = m_viewCorners[0].DistanceTo(m_viewCorners[2]);

            DrawRect = new Rectangle3d(m_drawPlane, m_viewWidth, m_viewHeight);
            UnitPerPx = viewport.Size.Width / m_viewWidth;
        }
    }
}
