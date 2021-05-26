/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    public class CurveRenderAttributeGoo : GH_Goo<CurveRenderAttribute>
    {
        public override bool IsValid => true;

        public override string TypeName => "Curve Render Attribute";

        public override string TypeDescription => "Curve Render Attribute";

        public CurveRenderAttributeGoo()
        {
        }

        public CurveRenderAttributeGoo(CurveRenderAttribute element)
            : base(element)
        {
        }

        public override IGH_Goo Duplicate()
        {
            if (Value == null)
            {
                return new CurveRenderAttributeGoo(null);
            }
            return new CurveRenderAttributeGoo(Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool CastFrom(object source)
        {
            CurveRenderAttribute element = source as CurveRenderAttribute;
            if (element != null)
            {
                Value = element;
                return true;
            }
            return false;
        }

        public override bool CastTo<TQ>(ref TQ target)
        {
            if (typeof(CurveRenderAttribute).IsAssignableFrom(typeof(TQ)))
            {
                target = (TQ)(object)Value;
                return true;
            }
            return false;
        }
    }
}
