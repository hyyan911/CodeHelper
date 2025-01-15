using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Drawing;
using System.Windows.Shapes;
using Rectangle = System.Windows.Shapes.Rectangle;
using Point = System.Windows.Point;

namespace CodeHelper
{
    public class WindowResizeHelper
    {
        Window window = null;

        double ResizeThickness = 4;
        double DragHeight = 20;

        /// <summary>
        /// 对给定窗口注册缩放事件
        /// </summary>
        /// <param name="window"></param>
        public void RegisterWindow(Window window, double resizeThickness = 4, double dragHeight = 20)
        {
            ResizeThickness = resizeThickness;
            this.DragHeight = dragHeight;
            this.window = window;
            // 为窗口的四个角落添加大小调整的触发器

            window.PreviewMouseLeftButtonDown += Window_MouseDown;
            window.PreviewMouseLeftButtonUp += Window_MouseUp;
            window.MouseMove += Window_MouseMove;
        }

        /// <summary>
        /// 清除注册事件
        /// </summary>
        public void ClearRegister()
        {
            if (window != null)
            {
                window.PreviewMouseLeftButtonDown -= Window_MouseDown;
                window.PreviewMouseLeftButtonUp -= Window_MouseUp;
                window.MouseMove -= Window_MouseMove;
            }
        }

        private Point clickPosition;
        private bool isResizingLeft = false;
        private bool isResizingRight = false;
        private bool isResizingTop = false;
        private bool isResizingBottom = false;

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isResizingLeft = false;
            isResizingTop = false;
            isResizingRight = false;
            isResizingBottom = false;
            clickPosition = e.GetPosition(window);
            if (Math.Abs(clickPosition.X) < ResizeThickness) isResizingLeft = true;
            if (Math.Abs(clickPosition.Y) < ResizeThickness) isResizingTop = true;
            if (Math.Abs(clickPosition.X - window.ActualWidth) < ResizeThickness) isResizingRight = true;
            if (Math.Abs(clickPosition.Y - window.ActualHeight) < ResizeThickness) isResizingBottom = true;
            if (isResizingBottom || isResizingLeft || isResizingRight || isResizingTop)
                window.CaptureMouse();
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isResizingRight = false;
            isResizingLeft = false;
            isResizingBottom = false;
            isResizingTop = false;
            window.ReleaseMouseCapture();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentPosition = e.GetPosition(window);
            window.Cursor = Cursors.Arrow;
            if (Math.Abs(currentPosition.X) < ResizeThickness)
                window.Cursor = Cursors.SizeWE;
            if (Math.Abs(currentPosition.Y) < ResizeThickness)
                window.Cursor = Cursors.SizeNS;
            if (Math.Abs(currentPosition.X - window.ActualWidth) < ResizeThickness)
                window.Cursor = Cursors.SizeWE;
            if (Math.Abs(currentPosition.Y - window.ActualHeight) < ResizeThickness)
                window.Cursor = Cursors.SizeNS;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (currentPosition.Y < DragHeight && currentPosition.Y > 0 && !isResizingTop)
                {
                    window.DragMove();
                    if (window.WindowState == WindowState.Maximized)
                    {
                        window.WindowState = WindowState.Normal;
                        window.Top = 0;
                    }
                    return;
                }
                if (isResizingLeft || isResizingRight || isResizingBottom || isResizingTop)
                {
                    double deltaX = currentPosition.X - clickPosition.X;
                    double deltaY = currentPosition.Y - clickPosition.Y;

                    if (isResizingLeft)
                    {
                        // 根据鼠标移动的位置调整窗口大小
                        window.Left += deltaX;
                        currentPosition = new Point(currentPosition.X - deltaX, currentPosition.Y);
                        window.Width = Double.IsNaN(window.ActualWidth) ? window.Width - deltaX : window.ActualWidth - deltaX;
                    }
                    if (isResizingRight)
                    {
                        // 根据鼠标移动的位置调整窗口大小
                        window.Width = Double.IsNaN(window.ActualWidth) ? window.Width + deltaX : window.ActualWidth + deltaX;
                    }
                    if (isResizingTop)
                    {
                        window.Top += deltaY;
                        currentPosition = new Point(currentPosition.X, currentPosition.Y - deltaY);
                        window.Height = Double.IsNaN(window.ActualHeight) ? window.Height - deltaY : window.ActualHeight - deltaY;
                    }
                    if (isResizingBottom)
                    {
                        window.Height = Double.IsNaN(window.ActualHeight) ? window.Height + deltaY : window.ActualHeight + deltaY;
                    }

                    // 更新点击位置
                    clickPosition = currentPosition;
                }
            }
        }
    }

}
