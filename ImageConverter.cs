using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CodeHelper
{
    /// <summary>
    /// 图片类型转换类
    /// </summary>
    public class ImageConverter
    {
        /// <summary>
        /// BitmapImage转Bitmap
        /// </summary>
        /// <param name="bitmapImage"></param>
        /// <returns></returns>
        public static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);
                return bitmap;
            }
        }

        /// <summary>
        /// Bitmap转BitmapImage
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);
        /// <summary>
        /// 位图转换
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapSource BitmapToBitmapSource(System.Drawing.Bitmap bitmap)
        {
            //BitmapImage bitmapImage = new BitmapImage();
            //using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            //{
            //    bitmap.Save(ms, ImageFormat.Bmp);
            //    bitmapImage.BeginInit();
            //    bitmapImage.StreamSource = ms;
            //    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            //    bitmapImage.EndInit();
            //    bitmapImage.Freeze();
            //}
            //bitmap.Dispose();
            IntPtr ip = bitmap.GetHbitmap();//从GDI+ Bitmap创建GDI位图对象

            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, Int32Rect.Empty,
            System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            bitmapSource.Freeze();
            DeleteObject(ip);//释放IntPtr,不然会引发内存泄漏
            return bitmapSource;
        }
    }
}
