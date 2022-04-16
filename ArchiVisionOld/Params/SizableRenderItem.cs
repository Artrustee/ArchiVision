﻿/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

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
    public abstract class SizableRenderItem :AttributeRenderItem
    {
        protected double InputSize { get; set; }

        public bool Absolute { get; protected set; }

        public SizableRenderItem(IGH_PreviewData geometry, double size, bool absolute, Color color = default(Color), bool topMost = false)
            :base(geometry, color, topMost)
        {
            InputSize = size;
            Absolute = absolute;
        }

        protected double GetSize(RhinoViewport viewport, double UnitPerPx)
        {
            double mult;
            if (IsTopMost)
            {
                double vpSize = 1;
                if (!Absolute) viewport.GetWorldToScreenScale(Geometry.ClippingBox.Center, out vpSize);
                mult = UnitPerPx / vpSize;
            }
            else
            {
                double vpSize = 1;
                if (Absolute) viewport.GetWorldToScreenScale(Geometry.ClippingBox.Center, out vpSize);
                mult = vpSize;
            }
            return mult * InputSize;
        }
    }
}