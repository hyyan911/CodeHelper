using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace CodeHelper
{
    /// <summary>
    /// 鼠标参数辅助类
    /// </summary>
    public class MouseHelper
    {
        private static double ratiox = Screen.PrimaryScreen.Bounds.Width / SystemParameters.PrimaryScreenWidth;
        private static double ratioy = Screen.PrimaryScreen.Bounds.Height / SystemParameters.PrimaryScreenHeight;

        /// <summary>
        /// 获取当前鼠标相对于屏幕的位置
        /// </summary>
        /// <returns></returns>
        public static Point GetCurrentMousePosition()
        {
            System.Drawing.Point p = System.Windows.Forms.Control.MousePosition;
            Point np = new Point(p.X / ratiox, p.Y / ratioy);
            return np;
        }
    }
}
