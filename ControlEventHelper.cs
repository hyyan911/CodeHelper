using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CodeHelper
{
    /// <summary>
    /// 对控件一些特殊事件的辅助类，目前包括控件的点击和双击事件
    /// </summary>
    public class ControlEventHelper
    {
        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        public event MouseButtonEventHandler Click = null;

        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        public event MouseButtonEventHandler MouseDoubleClick = null;

        private FrameworkElement element = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ele"></param>
        public ControlEventHelper(FrameworkElement ele)
        {
            element = ele;

            element.MouseLeftButtonDown += Click_Event;
            element.MouseLeftButtonDown += DClick_Event;
            element.MouseLeftButtonUp += Click_Up_Event;
        }

        private int clickTime = 0;

        private void Click_Up_Event(object sender, MouseButtonEventArgs e)
        {
            clickTime = e.Timestamp - clickTime;
            if (clickTime < 500)
            {
                Click?.Invoke(element, e);
            }
        }

        private void Click_Event(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            clickTime = e.Timestamp;
        }

        private bool isDoubleClick = false;
        private int firstClickTime = 0;

        private void DClick_Event(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (isDoubleClick == false)
            {
                firstClickTime = e.Timestamp;
                isDoubleClick = true;
                return;
            }
            else
            {
                if (e.Timestamp - firstClickTime < 300)
                {
                    MouseDoubleClick?.Invoke(element, e);
                }
                isDoubleClick = false;
            }
        }
    }
}
