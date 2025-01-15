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
    /// 水平固定模式
    /// </summary>
    public enum HorizontalAnchorMode
    {
        /// <summary>
        /// 左固定
        /// </summary>
        Left = 0,
        /// <summary>
        /// 右固定
        /// </summary>
        Right = 1,
        /// <summary>
        /// 不固定
        /// </summary>
        None = 2
    }
    /// <summary>
    /// 垂直固定模式
    /// </summary>
    public enum VerticalAnchorMode
    {
        /// <summary>
        /// 上固定
        /// </summary>
        Top = 0,
        /// <summary>
        /// 下固定
        /// </summary>
        Bottom = 1,
        /// <summary>
        /// 不固定
        /// </summary>
        None = 2
    }

    /// <summary>
    /// 当窗口尺寸改变时控件的缩放方式
    /// </summary>
    public enum ExpandMode
    {
        /// <summary>
        /// 按比例缩放
        /// </summary>
        Ratio = 0,
        /// <summary>
        /// 按缩放量缩放
        /// </summary>
        Offset = 1,
        /// <summary>
        /// 不缩放
        /// </summary>
        None = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public class RButton : Button, ResizeInterface
    {
        /// <inheritdoc/>
        public HorizontalAnchorMode HorizontalAnchor { get; set; } = HorizontalAnchorMode.Left;
        /// <inheritdoc/>
        public VerticalAnchorMode VerticalAnchor { get; set; } = VerticalAnchorMode.Top;
        /// <inheritdoc/>
        public Size HistorySize { get; set; }
        /// <inheritdoc/>
        public Point HistoryLoc { get; set; }
        /// <inheritdoc/>
        public ExpandMode HorizontalExpandMode { get; set; } = ExpandMode.None;
        /// <inheritdoc/>
        public ExpandMode VerticalalExpandMode { get; set; } = ExpandMode.None;
        /// <inheritdoc/>
        public double HorizontalOffsetRatio { get; set; } = 1;
        /// <inheritdoc/>
        public double VerticalOffsetRatio { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        public RButton()
        {
            Loaded += ControlLoaded;
        }

        private void ControlLoaded(object sender, RoutedEventArgs e)
        {
            HistorySize = new Size(ActualWidth, ActualHeight);
        }

        /// <inheritdoc/>
        internal void UpdateLocAndSize()
        {
            HistorySize = new Size(ActualWidth, ActualHeight);
            HistoryLoc = new Point(RCanvas.GetLeft(this), RCanvas.GetTop(this));
        }
    }
}
