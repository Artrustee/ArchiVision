/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    public class RenderItemGoo : GH_Goo<BaseRenderItem>
    { 
        public override bool IsValid => Value != null;

        public override string TypeName => "Render Item";

        public override string TypeDescription => "Render Item";

        public RenderItemGoo()
        {
        }

        public RenderItemGoo(BaseRenderItem element)
            : base(element)
        {
        }

        public override IGH_Goo Duplicate()
        {
            if (Value == null)
            {
                return new RenderItemGoo(null);
            }
            return new RenderItemGoo(Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool CastFrom(object source)
        {
            BaseRenderItem element = source as BaseRenderItem;
            if (element != null)
            {
                Value = element;
                return true;
            }
            return false;
        }

        public override bool CastTo<TQ>(ref TQ target)
        {
            if (typeof(BaseRenderItem).IsAssignableFrom(typeof(TQ)))
            {
                target = (TQ)(object)Value;
                return true;
            }
            return false;
        }
    }
}
