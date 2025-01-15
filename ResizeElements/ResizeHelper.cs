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
    /// 
    /// </summary>
    public class ResizeHelper
    {
        /// <summary>
        /// 缩放方法
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parent"></param>
        internal static void ResizeMethod(ResizeInterface obj, ResizeInterface parent)
        {
            if (!(parent is FrameworkElement) || !(obj is FrameworkElement)) return;
            if (!(parent is RCanvas)) return;
            RCanvas canvas = parent as RCanvas;

            double partialW = 0, partialH = 0;

            if (obj is RCanvas)
            {
                foreach (var item in (obj as RCanvas).Children)
                {
                    (item as ResizeInterface).HistorySize = new Size((item as FrameworkElement).ActualWidth, (item as FrameworkElement).ActualHeight);
                    (item as ResizeInterface).HistoryLoc = new Point(RCanvas.GetLeft(item as FrameworkElement), RCanvas.GetTop(item as FrameworkElement));
                }
            }

            //计算所占比例
            obj.HistorySize = new Size((obj as FrameworkElement).ActualWidth, (obj as FrameworkElement).ActualHeight);

            if (obj.HorizontalExpandMode == ExpandMode.Ratio)
            {
                partialW = obj.HistorySize.Width / parent.HistorySize.Width;
                (obj as FrameworkElement).Width = partialW * (parent as FrameworkElement).ActualWidth;
            }
            if (obj.HorizontalExpandMode == ExpandMode.Offset)
            {
                (obj as FrameworkElement).Width = (obj as FrameworkElement).ActualWidth + obj.HorizontalOffsetRatio * ((parent as FrameworkElement).ActualWidth - parent.HistorySize.Width);
            }
            if (obj.VerticalalExpandMode == ExpandMode.Ratio)
            {
                partialH = obj.HistorySize.Height / parent.HistorySize.Height;
                (obj as FrameworkElement).Height = partialH * (parent as FrameworkElement).ActualHeight;
            }
            if (obj.VerticalalExpandMode == ExpandMode.Offset)
            {
                (obj as FrameworkElement).Height = (obj as FrameworkElement).ActualHeight + obj.VerticalOffsetRatio * ((parent as FrameworkElement).ActualHeight - parent.HistorySize.Height);
            }

            (obj as FrameworkElement).UpdateLayout();

            #region 刷新控件位置
            //刷新控件位置
            if (obj.HorizontalAnchor == HorizontalAnchorMode.Right)
            {
                double right = parent.HistorySize.Width - obj.HistoryLoc.X - obj.HistorySize.Width;
                Canvas.SetLeft(obj as FrameworkElement, (parent as FrameworkElement).ActualWidth - right - (obj as FrameworkElement).ActualWidth);
            };
            if (obj.HorizontalAnchor == HorizontalAnchorMode.Left)
            {
                Canvas.SetLeft(obj as FrameworkElement, obj.HistoryLoc.X);
            };
            if (obj.VerticalAnchor == VerticalAnchorMode.Bottom)
            {
                double top = parent.HistorySize.Height - obj.HistoryLoc.Y - obj.HistorySize.Height;
                Canvas.SetTop(obj as FrameworkElement, (parent as FrameworkElement).ActualHeight - top - (obj as FrameworkElement).ActualHeight);
            };
            if (obj.VerticalAnchor == VerticalAnchorMode.Top)
            {
                Canvas.SetTop(obj as FrameworkElement, obj.HistoryLoc.Y);
            };
            #endregion            

            (obj as FrameworkElement).UpdateLayout();

            //如果是容器则刷新子控件
            if (obj is RCanvas)
            {
                foreach (var item in (obj as RCanvas).Children)
                {
                    if (!(item is ResizeInterface)) continue;
                    (item as ResizeInterface).HistorySize = new Size((item as FrameworkElement).ActualWidth, (item as FrameworkElement).ActualHeight);
                    (item as ResizeInterface).HistoryLoc = new Point(RCanvas.GetLeft(item as FrameworkElement), RCanvas.GetTop(item as FrameworkElement));
                    ResizeMethod((item as ResizeInterface), obj);
                }
            }
            if (obj is RBorder)
            {
                var ele = (obj as RBorder).Child;
                (ele as ResizeInterface).HistorySize = new Size((ele as FrameworkElement).Width, (ele as FrameworkElement).Height);

                if (ele is RCanvas)
                {
                    InnerResize(ele as ResizeInterface, obj);
                    foreach (var item in (ele as RCanvas).Children)
                    {
                        if (!(item is ResizeInterface)) continue;
                        (item as ResizeInterface).HistorySize = new Size((item as FrameworkElement).ActualWidth, (item as FrameworkElement).ActualHeight);
                        (item as ResizeInterface).HistoryLoc = new Point(RCanvas.GetLeft(item as FrameworkElement), RCanvas.GetTop(item as FrameworkElement));
                        ResizeMethod(item as ResizeInterface, ele as RCanvas);
                    }
                }
                else
                {
                    InnerResize(ele as ResizeInterface, obj);
                }
            }
            if (obj is RScrollViewer)
            {
                var ele = (obj as RScrollViewer).Content;
                (ele as ResizeInterface).HistorySize = new Size((ele as FrameworkElement).Width, (ele as FrameworkElement).Height);

                if (ele is RCanvas)
                {
                    InnerResize(ele as ResizeInterface, obj);
                    foreach (var item in (ele as RCanvas).Children)
                    {
                        if (!(item is ResizeInterface)) continue;
                        (item as ResizeInterface).HistorySize = new Size((item as FrameworkElement).ActualWidth, (item as FrameworkElement).ActualHeight);
                        (item as ResizeInterface).HistoryLoc = new Point(RCanvas.GetLeft(item as FrameworkElement), RCanvas.GetTop(item as FrameworkElement));
                        ResizeMethod(item as ResizeInterface, ele as RCanvas);
                    }
                }
                else
                {
                    InnerResize(ele as ResizeInterface, obj);
                }
            }
        }

        private static void InnerResize(ResizeInterface obj, ResizeInterface parent)
        {
            double partialW = 0, partialH = 0;
            if (obj.HorizontalExpandMode == ExpandMode.Ratio)
            {
                partialW = obj.HistorySize.Width / parent.HistorySize.Width;
                (obj as FrameworkElement).Width = partialW * (parent as FrameworkElement).ActualWidth;
            }
            if (obj.HorizontalExpandMode == ExpandMode.Offset)
            {
                (obj as FrameworkElement).Width = (obj as FrameworkElement).ActualWidth + obj.HorizontalOffsetRatio * ((parent as FrameworkElement).ActualWidth - parent.HistorySize.Width);
            }
            if (obj.VerticalalExpandMode == ExpandMode.Ratio)
            {
                partialH = obj.HistorySize.Height / parent.HistorySize.Height;
                (obj as FrameworkElement).Height = partialH * (parent as FrameworkElement).Height;
            }
            if (obj.VerticalalExpandMode == ExpandMode.Offset)
            {
                (obj as FrameworkElement).Height = (obj as FrameworkElement).ActualHeight + obj.VerticalOffsetRatio * ((parent as FrameworkElement).ActualHeight - parent.HistorySize.Height);
            }
            (obj as FrameworkElement).UpdateLayout();
        }
    }
}
