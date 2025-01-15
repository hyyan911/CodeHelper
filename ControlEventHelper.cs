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
            element.MouseLeftButtonDown += DClick_2_Event;
            element.MouseLeftButtonUp += Click_Up_Event;
        }

        private int clickTime = 0;

        private void Click_Up_Event(object sender, MouseButtonEventArgs e)
        {
            clickTime = e.Timestamp - clickTime;
            if (clickTime < 300)
            {
                Click?.Invoke(element, e);
            }
        }

        private void Click_Event(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            clickTime = e.Timestamp;
        }

        private int doubleClickTime = 0;
        private void DClick_2_Event(object sender, MouseButtonEventArgs e)
        {
            doubleClickTime = e.Timestamp - doubleClickTime;
            if (doubleClickTime < 300)
            {
                MouseDoubleClick?.Invoke(element, e);
            }
        }

        private void DClick_Event(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            doubleClickTime = e.Timestamp;
        }
    }
}
