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
        public List<Param_DisplayItem> ItemParams { get; } = new List<Param_DisplayItem>();
        //public List<RhinoViewPropertyComponent> PropertyComponents { get; } = new List<RhinoViewPropertyComponent>();

        /// <summary>
        /// Rectangle for drawing the top most display items.
        /// </summary>
        public Rectangle3d DrawRect { get; protected set; }

        public List<DisplayItem> DisplayItems 
        { 
            get
            {
                List<DisplayItem> result = new List<DisplayItem>();

                foreach (var item in ItemParams)
                {
                    result.AddRange(item.FindItems());
                }
                return result;
            }
        }

        public double UnitPerPx { get; protected set; }

        protected override void CalculateBoundingBox(CalculateBoundingBoxEventArgs e)
        {
            UpdateDrawInfo(e.Viewport);

            //Add the Draw Plane to the Bounding box.
            BoundingBox box = BoundingBox.Empty;
            DrawRect.ToNurbsCurve().Offset(DrawRect.Plane, Math.Max(DrawRect.Width, DrawRect.Height) / 4, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance, CurveOffsetCornerStyle.Sharp).ToList().ForEach((curve) =>
            {
                box.Union(curve.GetBoundingBox(false));
            });

            e.IncludeBoundingBox(box);

            // Add every display item's clipping box.
            BoundingBox b = BoundingBox.Empty;
            DisplayItems.ForEach((item) => b.Union(item.ClippingBox));

            e.IncludeBoundingBox(b);
            base.CalculateBoundingBox(e);
        }


        protected override void DrawForeground(DrawEventArgs e)
        {
            DisplayItems.ForEach((item) =>
            {
                item.DrawViewportWires(e, DrawRect, UnitPerPx);
                item.DrawViewportMeshes(e, DrawRect, UnitPerPx);
            });
            base.DrawForeground(e);
        }

        public void UpdateDrawInfo(RhinoViewport viewport)
        {
            // get the current view corners
            Point3d[] m_farCorners = viewport.GetFarRect();
            Point3d[] m_nearCorners = viewport.GetNearRect();

            Point3d[] m_viewCorners = new Point3d[3]
            {
                (m_farCorners[0] * 0.5 + m_nearCorners[0] * 0.5),
                (m_farCorners[1] * 0.5 + m_nearCorners[1] * 0.5),
                (m_farCorners[2] * 0.5 + m_nearCorners[2] * 0.5)
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
