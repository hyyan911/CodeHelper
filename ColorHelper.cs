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
            Random r = new Random();
            byte R = (byte)r.Next(256);
            byte G = (byte)r.Next(256);
            byte B = (byte)r.Next(256);
            while (Math.Pow(CenterColor.R - R, 2) + Math.Pow(CenterColor.G - G, 2) + Math.Pow(CenterColor.B - B, 2) < 50)
            {
                R = (byte)r.Next(256);
                G = (byte)r.Next(256);
                B = (byte)r.Next(256);
            }
            return Color.FromRgb(R, G, B);
        }
    }
}
