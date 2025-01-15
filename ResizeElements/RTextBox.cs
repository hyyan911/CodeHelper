using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CodeHelper.ResizeElements
{
    public class RTextBox : TextBox, ResizeInterface
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
        public RTextBox()
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
