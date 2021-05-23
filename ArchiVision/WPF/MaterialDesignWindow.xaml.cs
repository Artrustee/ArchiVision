using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ArchiVision
{
    /// <summary>
    /// Interaction logic for MaterialDesignWindow.xaml
    /// </summary>
    public partial class MaterialDesignWindow : Window
    {
        public MaterialDesignWindow()
        {
            InitializeComponent();
            Topmost = true;
        }

        public void SetColorTheme(System.Drawing.Color primaryColor, System.Drawing.Color secondaryColor)
        {
            CustomColorTheme colorTheme = new CustomColorTheme();
            colorTheme.PrimaryColor = Color.FromArgb(primaryColor.A, primaryColor.R, primaryColor.G, primaryColor.B);
            colorTheme.SecondaryColor = Color.FromArgb(secondaryColor.A, secondaryColor.R, secondaryColor.G, secondaryColor.B);
            this.Resources.MergedDictionaries.Add(colorTheme);
        }
    }
}
