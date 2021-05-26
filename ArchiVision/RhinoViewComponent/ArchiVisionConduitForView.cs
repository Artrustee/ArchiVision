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

    public class ConduitBase: DisplayConduit
    {
        protected void CaBoundingBox(CalculateBoundingBoxEventArgs e, List<BaseRenderItem> _items)
        {
            Point3d[] m_farCorners = e.Viewport.GetFarRect();
            Point3d[] m_nearCorners = e.Viewport.GetNearRect();

            Point3d[] m_viewCorners = new Point3d[4]
            {
                (m_farCorners[0] * 0.15 + m_nearCorners[0] * 0.85),
                (m_farCorners[1] * 0.15 + m_nearCorners[1] * 0.85),
                (m_farCorners[2] * 0.15 + m_nearCorners[2] * 0.85),
                (m_farCorners[3] * 0.15 + m_nearCorners[3] * 0.85)
                };

            BoundingBox box = new BoundingBox(m_viewCorners);

            e.IncludeBoundingBox(box);

            if (_items == null) return;

            BoundingBox b = BoundingBox.Empty;
            _items.ForEach((item) => b.Union(item.Geometry.ClippingBox));

            e.IncludeBoundingBox(b);
        }

        protected void DrawFore(DrawEventArgs e, List<BaseRenderItem> _items)
        {
            if (_items == null) return;
            _items.ForEach((item) =>
            {
                item.DrawViewportWires(e);
                item.DrawViewportMeshes(e);
            });
        }
    }

    public class ArchiVisionConduitForView : ConduitBase
    {
        public RhinoViewPropertyComponent Owner { get; set; }

        public ArchiVisionConduitForView(RhinoViewPropertyComponent owner)
        {
            Owner = owner;
        }

        protected override void CalculateBoundingBox(CalculateBoundingBoxEventArgs e)
        {
            base.CalculateBoundingBox(e);

            CaBoundingBox(e, Owner.FindItems(e.Viewport));
        }


        protected override void DrawForeground(DrawEventArgs e)
        {
            base.DrawForeground(e);
            DrawFore(e, Owner.FindItems(e.Viewport));
        }
    }

    //public class ArchiVisionConduitForParam<T> : ConduitBase where T : IGH_PreviewData
    //{
    //    public RenderItemParameter<T> Owner { get; set; }

    //    public ArchiVisionConduitForParam(RenderItemParameter<T> owner)
    //    {
    //        Owner = owner;
    //    }

    //    protected override void CalculateBoundingBox(CalculateBoundingBoxEventArgs e)
    //    {
    //        base.CalculateBoundingBox(e);
    //        CaBoundingBox(e, Owner.FindItems());
    //    }

    //    protected override void DrawForeground(DrawEventArgs e)
    //    {
    //        base.DrawForeground(e);
    //        DrawFore(e, Owner.FindItems());
    //    }
    //}
}
