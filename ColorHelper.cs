using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CodeHelper
{
    public class ColorHelper
    {
        private static Random random = new Random();

        /// <summary>
        /// 获取与指定颜色差距较大的随机颜色
        /// </summary>
        /// <returns></returns>

        public static Color GenerateHighContrastColor(Color CenterColor)
        {
            byte R = (byte)random.Next(256);
            byte G = (byte)random.Next(256);
            byte B = (byte)random.Next(256);
            double lum = GetContrastColor(CenterColor.R, CenterColor.G, CenterColor.B);
            while (Math.Abs(lum - GetContrastColor(R, G, B)) < 50)
            {
                R = (byte)random.Next(256);
                G = (byte)random.Next(256);
                B = (byte)random.Next(256);
            }
            return Color.FromRgb(R, G, B);
        }

        /// <summary>
        /// 根据亮度选择对比色（黑色或白色）
        /// </summary>
        /// <param name="color">原始颜色</param>
        /// <returns>对比色</returns>
        private static double GetContrastColor(byte R, byte G, byte B)
        {
            // 计算亮度
            return 0.299 * R + 0.587 * G + 0.114 * B;

        }
    }
}

