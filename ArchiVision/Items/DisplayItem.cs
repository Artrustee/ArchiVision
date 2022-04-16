/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
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
    public abstract class DisplayItem
    {

        protected List<DisplayItem> SubRenderItem { get; } = new List<DisplayItem>();
        public bool IsTopMost { get; }

        public virtual BoundingBox ClippingBox 
        {
            get
            {
                BoundingBox box = BoundingBox.Empty;
                SubRenderItem.ForEach((item) => box.Union(item.ClippingBox));
                return box;
            }
        }

        public DisplayItem(bool topMost = false)
        {
            IsTopMost = topMost;
        }

        public virtual void DrawViewportWires( RhinoViewport Viewport, DisplayPipeline Display,
            Rectangle3d drawRect, double unitPerPx, Color WireColour_Selected, DisplayMaterial ShadeMaterial_Selected, bool selected)
        {

        }

        public virtual void DrawViewportMeshes( RhinoViewport Viewport, DisplayPipeline Display,
            Rectangle3d drawRect, double unitPerPx, Color WireColour_Selected, DisplayMaterial ShadeMaterial_Selected, bool selected)
        {

        }

        public void DrawViewportWires(IGH_PreviewArgs args, bool selected) 
        {
            SubRenderItem.ForEach((sub) => sub.DrawViewportWires(args, selected));
            if (IsTopMost) return;
            DrawViewportWires(args.Viewport, args.Display, default(Rectangle3d), double.NaN, args.WireColour_Selected, args.ShadeMaterial_Selected, selected);
        }

        public void DrawViewportMeshes(IGH_PreviewArgs args, bool selected) 
        {
            SubRenderItem.ForEach((sub) => sub.DrawViewportMeshes(args, selected));
            if (IsTopMost) return;
            DrawViewportMeshes(args.Viewport, args.Display, default(Rectangle3d), double.NaN, args.WireColour_Selected, args.ShadeMaterial_Selected, selected);

        }

        public void DrawViewportWires(DrawEventArgs e, Rectangle3d drawRect, double unitPerPx)
        {
            SubRenderItem.ForEach((sub) => sub.DrawViewportWires(e, drawRect, unitPerPx));
            if (!IsTopMost) return;
            DrawViewportWires(e.Viewport, e.Display, drawRect, unitPerPx, Color.White, null, false);

        }

        public void DrawViewportMeshes(DrawEventArgs e, Rectangle3d drawRect, double unitPerPx)
        {
            SubRenderItem.ForEach((sub) => sub.DrawViewportMeshes(e, drawRect, unitPerPx));
            if (!IsTopMost) return;
            DrawViewportMeshes(e.Viewport, e.Display, drawRect, unitPerPx, Color.White, null, false);

        }

    }
}
