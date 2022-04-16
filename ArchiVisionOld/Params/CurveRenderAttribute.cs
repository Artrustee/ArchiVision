/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    public class CurveRenderAttribute
    {
        public Color Colour { get; set; } = Color.White;
        public double Thickness { get; set; } = 2;
        public Linetype LineType { get; set; }= Rhino.RhinoDoc.ActiveDoc.Linetypes.FindIndex(-1);
        public bool Absolute { get; set; } = true;

        public bool TopMost { get; set; } = false;

        public CurveRenderAttribute()
        {
        }

        public CurveRenderAttribute(Color color, double thickness, Linetype linetype, bool absolute, bool topMost)
        {
            this.Colour = color;
            this.Thickness = thickness;
            this.LineType = linetype;
            this.Absolute = absolute;
            this.TopMost = topMost;
        }

        //public CurveRenderAttribute(CurveRenderAttribute other)
        //{
        //    this.Colour = other.Colour;
        //    this.Thickness = other.Thickness;
        //    this.LineType = other.LineType;
        //    this.Absolute = other.Absolute;
        //}
    }
}
