using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CodeHelper.ResizeElements
{
    /// <summary>
    /// 窗口内控件缩放辅助类，当控件设置为继承自ResizeInterface接口的自定义类时可以规定多种缩放方式，跟随窗口大小一起缩放
    /// </summary>
    public class WindowResizeHelper
    {
        private Window MainWindow { get; set; } = null;

        /// <summary>
        /// 窗口尺寸
        /// </summary>
        private Size windowSize = new Size();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetwindow"></param>
        public WindowResizeHelper(Window targetwindow)
        {
            MainWindow = targetwindow;
            if (targetwindow.Content is RCanvas)
            {
                MainWindow.Loaded += LoadedEvent;
                MainWindow.SizeChanged += SizeChangedEvent;
            }
        }

        private void SizeChangedEvent(object sender, SizeChangedEventArgs e)
        {
            if (MainWindow.IsLoaded == false) { return; }
            RCanvas c = MainWindow.Content as RCanvas;
            c.HistorySize = new Size(c.ActualWidth, c.ActualHeight);
            c.Width = c.ActualWidth + MainWindow.ActualWidth - windowSize.Width;
            c.Height = c.ActualHeight + MainWindow.ActualHeight - windowSize.Height;
            MainWindow.UpdateLayout();
            windowSize = new Size(MainWindow.ActualWidth, MainWindow.ActualHeight);
            MainWindow.UpdateLayout();
            foreach (var item in c.Children)
            {
                if (!(item is ResizeInterface)) continue;
                (item as ResizeInterface).HistorySize = new Size((item as FrameworkElement).ActualWidth, (item as FrameworkElement).ActualHeight);
                (item as ResizeInterface).HistoryLoc = new Point(RCanvas.GetLeft(item as FrameworkElement), RCanvas.GetTop(item as FrameworkElement));
                ResizeHelper.ResizeMethod(item as ResizeInterface, MainWindow.Content as ResizeInterface);
            }
        }

        private void LoadedEvent(object sender, RoutedEventArgs e)
        {
            windowSize = new Size(MainWindow.ActualWidth, MainWindow.ActualHeight);
            RCanvas c = MainWindow.Content as RCanvas;
            c.HistorySize = new Size(c.ActualWidth, c.ActualHeight);
            foreach (var item in c.Children)
            {
                if (!(item is ResizeInterface)) continue;
                (item as ResizeInterface).HistorySize = new Size((item as FrameworkElement).ActualWidth, (item as FrameworkElement).ActualHeight);
                (item as ResizeInterface).HistoryLoc = new Point(RCanvas.GetLeft(item as FrameworkElement), RCanvas.GetTop(item as FrameworkElement));
                ResizeHelper.ResizeMethod(item as ResizeInterface, MainWindow.Content as ResizeInterface);
            }
        }
    }
}
