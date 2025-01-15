using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Media;
using System.Windows.Interop;
using Point = System.Windows.Point;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Window = System.Windows.Window;

namespace CodeHelper
{
    /// <summary>
    /// 截图工具
    /// </summary>
    public class SnapHelper
    {
        /// <summary>
        /// 获取屏幕截图
        /// </summary>
        /// <returns></returns>
        public static BitmapSource GetScreenShot()
        {
            //创建与屏幕大小相同的位图对象
            var bmpScreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            //使用位图对象来创建Graphics的对象
            using (Graphics g = Graphics.FromImage(bmpScreen))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;   //设置平滑模式，抗锯齿
                g.CompositingQuality = CompositingQuality.HighQuality;  //设置合成质量
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;     //设置插值模式
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;   //设置文本呈现的质量
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;    //设置呈现期间，像素偏移的方式

                //利用CopyFromScreen将当前屏幕截图并将内容存储在bmpScreen的位图中
                g.CopyFromScreen(0, 0, 0, 0, bmpScreen.Size, CopyPixelOperation.SourceCopy);
            }

            return ImageConverter.BitmapToBitmapSource(bmpScreen);
        }

        /// <summary>
        /// 获取某一区域的截图
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static BitmapSource GetScreenShot(Rect area)
        {
            double ratiox = Screen.PrimaryScreen.Bounds.Width / SystemParameters.PrimaryScreenWidth;
            double ratioy = Screen.PrimaryScreen.Bounds.Height / SystemParameters.PrimaryScreenHeight;
            Rect transformedarea = new Rect(area.X, area.Y, area.Width, area.Height);

            //创建与屏幕大小相同的位图对象
            var bmpScreen = new Bitmap((int)transformedarea.Width, (int)transformedarea.Height);

            //使用位图对象来创建Graphics的对象
            using (Graphics g = Graphics.FromImage(bmpScreen))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;   //设置平滑模式，抗锯齿
                g.CompositingQuality = CompositingQuality.HighQuality;  //设置合成质量
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;     //设置插值模式
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;   //设置文本呈现的质量
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;    //设置呈现期间，像素偏移的方式

                //利用CopyFromScreen将当前屏幕截图并将内容存储在bmpScreen的位图中
                g.CopyFromScreen((int)transformedarea.X, (int)transformedarea.Y, 0, 0, bmpScreen.Size, CopyPixelOperation.SourceCopy);
            }

            return ImageConverter.BitmapToBitmapSource(bmpScreen);
        }

        /// <summary>
        /// 获取指定控件的截图
        /// </summary>
        /// <returns></returns>
        public static BitmapSource GetControlSnap(FrameworkElement control)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)control.ActualWidth, (int)control.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(control);
            return renderTargetBitmap;
        }

        /// <summary>
        /// 获取指定控件的截图
        /// </summary>
        /// <returns></returns>
        public static BitmapSource GetControlAreaSnap(FrameworkElement control)
        {
            return GetScreenShot(new Rect(control.PointToScreen(new Point(0, 0)), control.PointToScreen(new Point(control.ActualWidth, control.ActualHeight))));
        }
    }
}
