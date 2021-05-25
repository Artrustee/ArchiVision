using MaterialDesignThemes.Wpf;
using Rhino.Display;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using System.Windows.Forms.Integration;
using RhinoWindows.Forms.Controls;

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
            SourceInitialized += MainWindow_SourceInitialized;

        }


        #region 给wpf标题栏的右键菜单栏 添加 “保存”菜单项
        private const int WM_SYSCOMMAND = 0x112;
        private const int MF_STRING = 0x0;
        private const int MF_SEPARATOR = 0x800;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InsertMenuItem(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, String ipNewItem);

        private int SYSMENU_ABOUT_ID = 0x1;

        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            var handle = (new WindowInteropHelper(this)).Handle;
            IntPtr hSysMenu = GetSystemMenu(handle, false);
            AppendMenu(hSysMenu, MF_SEPARATOR, 0, String.Empty);
            AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUT_ID, "Save Picture");
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
        }
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Handle messages...  
            if ((msg == WM_SYSCOMMAND) && ((int)wParam == SYSMENU_ABOUT_ID))
            {
                SaveElementPicture();
            }
            return IntPtr.Zero;
        }
        #endregion

        #region Save Picture
        public void SaveElementPicture()
        {
            ExportSaveFileDialog("Picture", (location) =>
            {
                MergeMap().Save(location);
            });
        }

        public void ExportSaveFileDialog(string defaultName, Action<string> saveAction)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();

            saveFileDialog.Title = "Save Picture";
            saveFileDialog.Filter = "*.JPG|*.JPG|.BMP|*.BMP";
            saveFileDialog.FileName = "Picture";
            saveFileDialog.SupportMultiDottedExtensions = false;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                saveAction.Invoke(saveFileDialog.FileName);
            }
        }

        public Bitmap MergeMap()
        {
            Bitmap bitmap = ToBitmap(this);
            Graphics g = Graphics.FromImage(bitmap);

            FindElement(new List<UIElement>() { this }, (host, view, i) =>
            {
                System.Windows.Point point = host.TransformToAncestor(Window.GetWindow(host)).Transform(new System.Windows.Point(0, 0));
                Bitmap bit = CaptureControl((ViewportControl)host.Child);
                g.DrawImage(bit, new System.Drawing.Point((int)point.X, (int)point.Y));
            });

            g.Dispose();
            return bitmap;
        }


        public Bitmap ToBitmap(UIElement visual)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)visual.RenderSize.Width, (int)visual.RenderSize.Height, 0, 0, PixelFormats.Default);
            renderTargetBitmap.Render(visual);
            return BitmapSourceToBitmap(renderTargetBitmap);
        }


        public bool FindElement(List<UIElement> elements, Action<WindowsFormsHost,RhinoViewport, int> viewportAction)
        {
            foreach (var item in elements)
            {
                if (Helper.IsViewport(item, viewportAction, false)) return true;
                else
                {
                    List<UIElement> lt = new List<UIElement>();
                    if (item is ContentControl)
                    {
                        lt.Add((UIElement)((item as ContentControl).Content));
                        return FindElement(lt, viewportAction);

                    }
                    else if (item is Panel)
                    {
                        foreach (var child in (item as Panel).Children)
                        {
                            lt.Add((UIElement)child);
                        }
                        return FindElement(lt, viewportAction);
                    }
                    else return false;
                }
            }
            return false;
        }

        #region  DLL calls
        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int
        wDest, int hDest, IntPtr hdcSource, int xSrc, int ySrc, CopyPixelOperation rop);
        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteDC(IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteObject(IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr ptr);
        [DllImport("user32.dll")]
        static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDc);
        #endregion

        /// <summary>
        /// 控件截图
        /// </summary>
        /// <param name="control">需要被截图的控件</param>
        /// <remarks>使用BitBlt方法，控件被遮挡时也可以正确截图</remarks>
        public static Bitmap CaptureControl(System.Windows.Forms.Control control)
        {
            IntPtr hSrce = GetWindowDC(control.Handle);
            IntPtr hDest = CreateCompatibleDC(hSrce);
            IntPtr hBmp = CreateCompatibleBitmap(hSrce, control.Width, control.Height);
            IntPtr hOldBmp = SelectObject(hDest, hBmp);
            if (BitBlt(hDest, 0, 0, control.Width, control.Height, hSrce, 0, 0, CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt))
            {
                Bitmap bmp = System.Drawing.Image.FromHbitmap(hBmp);
                SelectObject(hDest, hOldBmp);
                DeleteObject(hBmp);
                DeleteDC(hDest);
                ReleaseDC(control.Handle, hSrce);
                // bmp.Save(@"a.png");
                // bmp.Dispose();
                return bmp;
            }
            return null;
        }

        /// <summary>
        /// BitmapSource转Bitmap
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Bitmap BitmapSourceToBitmap(BitmapSource source)
        {
            return BitmapSourceToBitmap(source, source.PixelWidth, source.PixelHeight);
        }

        /// <summary>
        /// Convert BitmapSource to Bitmap
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Bitmap BitmapSourceToBitmap(BitmapSource source, int width, int height)
        {
            Bitmap bmp = null;
            try
            {
                PixelFormat format = PixelFormat.Format24bppRgb;
                /*set the translate type according to the in param(source)*/
                switch (source.Format.ToString())
                {
                    case "Rgb24":
                    case "Bgr24": format = PixelFormat.Format24bppRgb; break;
                    case "Bgra32": format = PixelFormat.Format32bppPArgb; break;
                    case "Bgr32": format = PixelFormat.Format32bppRgb; break;
                    case "Pbgra32": format = PixelFormat.Format32bppArgb; break;
                }
                bmp = new Bitmap(width, height, format);
                BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size),
                    ImageLockMode.WriteOnly,
                    format);
                source.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
                bmp.UnlockBits(data);
            }
            catch
            {
                if (bmp != null)
                {
                    bmp.Dispose();
                    bmp = null;
                }
            }

            return bmp;
        }
        #endregion
    }
}
