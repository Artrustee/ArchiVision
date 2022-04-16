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
    public abstract class SizableDisplayIItem :AttributeDisplayItem
    {
        protected double InputSize { get; set; }

        public bool Absolute { get; protected set; }

        public SizableDisplayIItem(IGH_PreviewData geometry, double size, bool absolute, Color color = default(Color), bool topMost = false)
            :base(geometry, color, topMost)
        {
            InputSize = size;
            Absolute = absolute;
        }

        protected double GetSize(RhinoViewport viewport, double UnitPerPx)
        {
            //double mult;
            //if (IsTopMost)
            //{
            //    double vpSize = 1;
            //    if (!Absolute) viewport.GetWorldToScreenScale(Geometry.ClippingBox.Center, out vpSize);
            //    mult = UnitPerPx / vpSize;
            //}
            //else
            //{
            //    double vpSize = 1;
            //    if (Absolute) viewport.GetWorldToScreenScale(Geometry.ClippingBox.Center, out vpSize);
            //    mult = vpSize;
            //}
            if (IsTopMost) return InputSize;
            else return UnitPerPx * InputSize;

        }
    }
}
