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
using Controls;
using System.ComponentModel;

namespace CodeHelper
{
    public class WindowResizeHelper
    {
        Window window = null;

        double ResizeThickness = 4;
        double DragHeight = 20;

        public event RoutedEventHandler BeforeHide = null;
        public event RoutedEventHandler AfterHide = null;


        public event RoutedEventHandler BeforeClose = null;
        public event RoutedEventHandler AfterClose = null;

        /// <summary>
        /// 对给定窗口注册缩放事件(关闭按钮按下关闭窗口)
        /// </summary>
        /// <param name="window"></param>
        public void RegisterCloseWindow(Window window, DecoratedButton minimunBtn, DecoratedButton maximunBtn, DecoratedButton closeBtn, DecoratedButton pinBtn = null, double resizeThickness = 4, double dragHeight = 20)
        {
            ResizeThickness = resizeThickness;
            this.DragHeight = dragHeight;
            this.window = window;
            // 为窗口的四个角落添加大小调整的触发器

            window.PreviewMouseLeftButtonDown += Window_MouseDown;
            window.PreviewMouseLeftButtonUp += Window_MouseUp;
            window.MouseMove += Window_MouseMove;
            if (minimunBtn != null)
            {
                minimunBtn.Cursor = Cursors.Hand;
                minimunBtn.Click -= Minimize;
                minimunBtn.Click += Minimize;
            }
            if (maximunBtn != null)
            {
                maximunBtn.Cursor = Cursors.Hand;
                maximunBtn.Click -= Maximize;
                maximunBtn.Click += Maximize;
            }
            if (closeBtn != null)
            {
                closeBtn.Cursor = Cursors.Hand;
                closeBtn.Click -= Close;
                closeBtn.Click += Close;
            }
            window.Closing -= ClosingEvent;
            window.Closed -= ClosedEvent;
            window.Closing += ClosingEvent;
            window.Closed += ClosedEvent;
            if (pinBtn != null)
            {
                pinBtn.Cursor = Cursors.Hand;
                pinBtn.Click -= PinEvent;
                pinBtn.Click += PinEvent;
            }
        }

        /// <summary>
        /// 对给定窗口注册缩放事件(关闭按钮按下隐藏窗口)
        /// </summary>
        /// <param name="window"></param>
        public void RegisterHideWindow(Window window, DecoratedButton minimunBtn, DecoratedButton maximunBtn, DecoratedButton closeBtn, DecoratedButton pinBtn = null, double resizeThickness = 4, double dragHeight = 20)
        {
            ResizeThickness = resizeThickness;
            this.DragHeight = dragHeight;
            this.window = window;
            // 为窗口的四个角落添加大小调整的触发器

            window.PreviewMouseLeftButtonDown += Window_MouseDown;
            window.PreviewMouseLeftButtonUp += Window_MouseUp;
            window.MouseMove += Window_MouseMove;
            if (minimunBtn != null)
            {
                minimunBtn.Cursor = Cursors.Hand;
                minimunBtn.Click -= Minimize;
                minimunBtn.Click += Minimize;
            }
            if (maximunBtn != null)
            {
                maximunBtn.Cursor = Cursors.Hand;
                maximunBtn.Click -= Maximize;
                maximunBtn.Click += Maximize;
            }
            if (closeBtn != null)
            {
                closeBtn.Cursor = Cursors.Hand;
                closeBtn.Click -= Hide;
                closeBtn.Click += Hide;
            }
            window.Closing -= CloseHide;
            window.Closing += CloseHide;
            if (pinBtn != null)
            {
                pinBtn.Cursor = Cursors.Hand;
                pinBtn.Click -= PinEvent;
                pinBtn.Click += PinEvent;
            }
        }

        private void PinEvent(object sender, RoutedEventArgs e)
        {
            var btn = sender as DecoratedButton;
            if (btn.KeepPressed == true)
            {
                btn.KeepPressed = false;
                window.Topmost = false;
            }
            else
            {
                btn.KeepPressed = true;
                window.Topmost = true;
            }
        }

        private void ClosingEvent(object sender, CancelEventArgs e)
        {
            BeforeClose?.Invoke(window, new RoutedEventArgs());
        }

        private void ClosedEvent(object sender, EventArgs e)
        {
            AfterClose?.Invoke(window, new RoutedEventArgs());
        }

        private void CloseHide(object sender, CancelEventArgs e)
        {
            BeforeHide?.Invoke(window, new RoutedEventArgs());
            window.Hide();
            AfterHide?.Invoke(window, new RoutedEventArgs());
            e.Cancel = true;
        }

        private void Hide(object sender, RoutedEventArgs e)
        {
            BeforeHide?.Invoke(window, e);
            window.Hide();
            AfterHide?.Invoke(window, e);
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            BeforeClose?.Invoke(window, new RoutedEventArgs());
            window.Close();
            AfterClose?.Invoke(window, new RoutedEventArgs());
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minimize(object sender, RoutedEventArgs e)
        {
            window.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Maximize(object sender, RoutedEventArgs e)
        {
            if (window.WindowState == WindowState.Maximized)
            {
                window.WindowState = WindowState.Normal;
                return;
            }
            if (window.WindowState == WindowState.Normal)
            {
                window.MaxHeight = SystemParameters.WorkArea.Height;
                window.MaxWidth = SystemParameters.WorkArea.Width;
                window.WindowState = WindowState.Maximized;
                return;
            }
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
                        var width = double.IsNaN(window.ActualWidth) ? window.Width - deltaX : window.ActualWidth - deltaX;
                        if (width < 0) width = 10;
                        window.Width = width;
                    }
                    if (isResizingRight)
                    {
                        // 根据鼠标移动的位置调整窗口大小
                        var width = double.IsNaN(window.ActualWidth) ? window.Width + deltaX : window.ActualWidth + deltaX;
                        if (width < 0) width = 10;
                        window.Width = width;
                    }
                    if (isResizingTop)
                    {
                        window.Top += deltaY;
                        currentPosition = new Point(currentPosition.X, currentPosition.Y - deltaY);
                        var height = double.IsNaN(window.ActualHeight) ? window.Height - deltaY : window.ActualHeight - deltaY;
                        if (height < 0) height = 10;
                        window.Height = height;
                    }
                    if (isResizingBottom)
                    {
                        var height = double.IsNaN(window.ActualHeight) ? window.Height + deltaY : window.ActualHeight + deltaY;
                        if (height < 0) height = 10;
                        window.Height = height;
                    }

                    // 更新点击位置
                    clickPosition = currentPosition;
                }
            }
        }
    }

}
