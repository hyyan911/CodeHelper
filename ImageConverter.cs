using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

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
            //    ms.Flush();
            //    ms.Close();
            //}
            //bitmap.Dispose();
            //GC.Collect();
            IntPtr ip = bitmap.GetHbitmap();//从GDI+ Bitmap创建GDI位图对象

            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, Int32Rect.Empty,
            System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(ip);//释放IntPtr,不然会引发内存泄漏
            return bitmapSource;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rgbaData"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap ConvertFromRGBA(byte[] rgbaData, int width, int height, PixelFormat format)
        {
            var pixelFormat = format;
            Bitmap bitmap = new Bitmap(width, height, pixelFormat);

            BitmapData bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.WriteOnly,
                pixelFormat);

            IntPtr intPtr = bitmapData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(rgbaData, 0, intPtr, rgbaData.Length);
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="source"></param>
        public static WriteableBitmap UpdateWritableBitmap(Bitmap bmp, WriteableBitmap source)
        {
            if (bmp == null) return null;

            bool IsFormatEqual = false;

            if (source == null || bmp.Width != source.PixelWidth || bmp.Height != source.PixelHeight || bmp.HorizontalResolution != source.DpiX || bmp.VerticalResolution != source.DpiY
                || ConvertPixelFormatToWpfPixelFormat(bmp.PixelFormat) != source.Format)
            {
                source = new WriteableBitmap(bmp.Width, bmp.Height, bmp.HorizontalResolution, bmp.VerticalResolution, ConvertPixelFormatToWpfPixelFormat(bmp.PixelFormat), null);
            }
            // 锁定 WriteableBitmap 的像素区域
            source.Lock();

            // 将 Bitmap 数据复制到 WriteableBitmap
            System.Drawing.Imaging.BitmapData bitmapData = bmp.LockBits(
                new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                bmp.PixelFormat);

            int bytesPerPixel = Bitmap.GetPixelFormatSize(bmp.PixelFormat) / 8;
            int byteCount = bitmapData.Stride * bmp.Height;

            byte[] bitmapBytes = new byte[byteCount];
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, bitmapBytes, 0, byteCount);

            System.Runtime.InteropServices.Marshal.Copy(bitmapBytes, 0, source.BackBuffer, byteCount);

            // 解锁 Bitmap 和 WriteableBitmap
            bmp.UnlockBits(bitmapData);
            source.Unlock();

            return source;
        }

        private static System.Windows.Media.PixelFormat ConvertPixelFormatToWpfPixelFormat(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format32bppArgb:
                    return PixelFormats.Bgra32;
                case PixelFormat.Format32bppRgb:
                    return PixelFormats.Bgr32;
                case PixelFormat.Format24bppRgb:
                    return PixelFormats.Bgr24;
                case PixelFormat.Format8bppIndexed:
                    return PixelFormats.Gray8;
                case PixelFormat.Format16bppRgb555:
                    return PixelFormats.Bgr555;
                case PixelFormat.Format16bppRgb565:
                    return PixelFormats.Bgr565;
                case PixelFormat.Format32bppPArgb:
                    return PixelFormats.Pbgra32;
                case PixelFormat.Format64bppArgb:
                    return PixelFormats.Prgba64;
                default:
                    throw new NotSupportedException($"Unsupported pixel format: {pixelFormat}");
            }
        }
    }
}
