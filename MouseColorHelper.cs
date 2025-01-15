using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Brush = System.Windows.Media.Brush;

namespace CodeHelper
{
    /// <summary>
    /// 控件鼠标颜色辅助类，负责鼠标移入，点击控件变色
    /// </summary>
    public class MouseColorHelper
    {
        private Brush IB { get; set; } = null;

        private Brush MB { get; set; } = null;

        private Brush PB { get; set; } = null;


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="InitBackground"></param>
        /// <param name="MoveBackground"></param>
        /// <param name="PressedBackground"></param>
        public MouseColorHelper(Brush InitBackground, Brush MoveBackground, Brush PressedBackground)
        {
            IB = InitBackground;
            MB = MoveBackground;
            PB = PressedBackground;
        }

        private void SetColor(FrameworkElement ele, Brush b)
        {
            if (ele is Control)
            {
                (ele as Control).Background = b;
            }
            if (ele is TextBlock)
            {
                (ele as TextBlock).Background = b;
            }
            if (ele is Panel)
            {
                (ele as Panel).Background = b;
            }
            if (ele is Border)
            {
                (ele as Border).Background = b;
            }
            if (ele is Shape)
            {
                (ele as Shape).Fill = b;
            }
        }

        /// <summary>
        /// 注册控件，附加颜色改变事件
        /// </summary>
        /// <param name="target"></param>
        public void RegistateTarget(FrameworkElement target)
        {
            target.PreviewMouseLeftButtonDown -= PressedEvent;
            target.PreviewMouseLeftButtonUp -= UpEvent;
            target.MouseEnter -= MoveEvent;
            target.MouseLeave -= LeaveEvent;

            target.PreviewMouseLeftButtonDown += PressedEvent;
            target.PreviewMouseLeftButtonUp += UpEvent;
            target.MouseEnter += MoveEvent;
            target.MouseLeave += LeaveEvent;

            SetColor(target, IB);
        }

        /// <summary>
        /// 注销目标
        /// </summary>
        /// <param name="target"></param>
        public void CancelEvent(FrameworkElement target)
        {
            target.PreviewMouseLeftButtonDown -= PressedEvent;
            target.PreviewMouseLeftButtonUp -= UpEvent;
            target.MouseEnter -= MoveEvent;
            target.MouseLeave -= LeaveEvent;
            SetColor(target, IB);
        }

        private void PressedEvent(object sender, MouseButtonEventArgs e)
        {
            SetColor(sender as FrameworkElement, PB);
        }
        private void MoveEvent(object sender, MouseEventArgs e)
        {
            SetColor(sender as FrameworkElement, MB);
        }
        private void LeaveEvent(object sender, MouseEventArgs e)
        {
            SetColor(sender as FrameworkElement, IB);
        }
        private void UpEvent(object sender, MouseButtonEventArgs e)
        {
            SetColor(sender as FrameworkElement, MB);
        }
    }
}
