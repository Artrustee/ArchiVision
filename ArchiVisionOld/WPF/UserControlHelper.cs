/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel.Special;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;

namespace ArchiVision
{
    public static class UserControlHelper
    {
        #region NumberSlider 还差Snapping没有成功联动起来。
        private static string FormatDec(GH_NumberSlider slider)
        {
            switch (slider.Slider.Type)
            {
                case Grasshopper.GUI.Base.GH_SliderAccuracy.Float:
                    return "F" + slider.Slider.DecimalPlaces.ToString();
                case Grasshopper.GUI.Base.GH_SliderAccuracy.Integer:
                    return "F0";
                default: return string.Empty;
            }
        }

        private static string SliderFormat(GH_NumberSlider slider, decimal value)
        {
            string dec = FormatDec(slider);
            if (string.IsNullOrEmpty(dec))
            {
                switch (slider.Slider.Type)
                {
                    case Grasshopper.GUI.Base.GH_SliderAccuracy.Even:
                        return (int.Parse((value / 2).ToString("F0")) * 2).ToString("F0");
                    case Grasshopper.GUI.Base.GH_SliderAccuracy.Odd:
                        return (int.Parse(((value - 1) / 2).ToString("F0")) * 2 + 1).ToString("F0");
                    default: throw new Exception();
                }
            }
            else
            {
                return value.ToString(dec);
            }
        }

        private static string SliderFormat(GH_NumberSlider slider, double value)
        {
            string dec = FormatDec(slider);
            if (string.IsNullOrEmpty(dec))
            {
                switch (slider.Slider.Type)
                {
                    case Grasshopper.GUI.Base.GH_SliderAccuracy.Even:
                        return (int.Parse((value / 2).ToString("F0")) * 2).ToString("F0");
                    case Grasshopper.GUI.Base.GH_SliderAccuracy.Odd:
                        return (int.Parse(((value - 1) / 2).ToString("F0")) * 2 + 1).ToString("F0");
                    default: throw new Exception();
                }
            }
            else
            {
                return value.ToString(dec);
            }
        }

        public static Grid NumberSlider(GH_NumberSlider slider)
        {

            Grid grid = new Grid();
            ColumnDefinition def1 = new ColumnDefinition();
            def1.Width = GridLength.Auto;

            ColumnDefinition def2 = new ColumnDefinition();
            def2.Width = (GridLength)new GridLengthConverter().ConvertFromString("*");

            ColumnDefinition def3 = new ColumnDefinition();
            def3.Width = GridLength.Auto;

            grid.ColumnDefinitions.Add(def1);
            grid.ColumnDefinitions.Add(def2);
            grid.ColumnDefinitions.Add(def3);

            TextBlock name = new TextBlock() { Text = slider.NickName, VerticalAlignment = VerticalAlignment.Center };

            Grid.SetColumn(name, 0);
            grid.Children.Add(name);

            Slider newslider = new Slider() {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Minimum = (double)slider.Slider.Minimum,
                Maximum = (double)slider.Slider.Maximum,
                Value = (double)slider.Slider.Value,
                Margin = new Thickness(8, 0, 8, 0),
            };

            Grid.SetColumn(newslider, 1);
            grid.Children.Add(newslider);


            TextBox valueBox = new TextBox()
            {
                Text = SliderFormat(slider, slider.Slider.Value),
                VerticalAlignment = VerticalAlignment.Center,
            };
            Grid.SetColumn(valueBox, 2);
            grid.Children.Add(valueBox);

            Action<decimal> DoIt = (number) =>
            {
                string numDec = SliderFormat(slider, number);
                if (SliderFormat(slider, slider.Slider.Value) != numDec) slider.Slider.Value = number;
                if (SliderFormat(slider, newslider.Value) != numDec) newslider.Value = (double)number;
                if (valueBox.Text != numDec) valueBox.Text = numDec;
            };

            slider.Slider.ValueChanged += (x, y) => DoIt.Invoke(y.Value);
            newslider.ValueChanged += (x, y) => DoIt.Invoke((decimal)y.NewValue);
            valueBox.TextChanged += (x, y) =>
            {
                decimal value;
                if (decimal.TryParse(valueBox.Text, out value)) DoIt.Invoke(value);
            };

            return grid;
        }
        #endregion
    }
}
