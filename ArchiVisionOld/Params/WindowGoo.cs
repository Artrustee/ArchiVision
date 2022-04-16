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
using System.Windows;

namespace ArchiVision
{
    public class WindowGoo : GH_Goo<MaterialDesignWindow>
    {
        public override bool IsValid => Value != null;

        public override string TypeName => "Window";

        public override string TypeDescription => "Window for WPF.";

        public WindowGoo()
        {
        }

        public WindowGoo(MaterialDesignWindow window)
            : base(window)
        {
        }

        public override IGH_Goo Duplicate()
        {
            if (Value == null)
            {
                return new WindowGoo(null);
            }
            return new WindowGoo(Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool CastFrom(object source)
        {
            MaterialDesignWindow element = source as MaterialDesignWindow;
            if (element != null)
            {
                Value = element;
                return true;
            }
            return false;
        }

        public override bool CastTo<TQ>(ref TQ target)
        {
            if (typeof(MaterialDesignWindow).IsAssignableFrom(typeof(TQ)))
            {
                target = (TQ)(object)Value;
                return true;
            }
            return false;
        }
    }
}
