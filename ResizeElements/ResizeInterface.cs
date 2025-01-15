using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CodeHelper.ResizeElements
{
    /// <summary>
    /// 
    /// </summary>
    public interface ResizeInterface
    {
        /// <summary>
        /// 水平固定方式
        /// </summary>
        HorizontalAnchorMode HorizontalAnchor { get; set; }
        /// <summary>
        /// 竖直固定方式
        /// </summary>
        VerticalAnchorMode VerticalAnchor { get; set; }

        /// <summary>
        /// 历史尺寸
        /// </summary>
        Size HistorySize { get; set; }
        /// <summary>
        /// 历史位置
        /// </summary>
        Point HistoryLoc { get; set; }

        /// <summary>
        /// 水平缩放比例系数
        /// </summary>
        double HorizontalOffsetRatio { get; set; }
        /// <summary>
        /// 垂直缩放比例系数
        /// </summary>
        double VerticalOffsetRatio { get; set; }


        /// <summary>
        /// 水平缩放模式
        /// </summary>
        ExpandMode HorizontalExpandMode { get; set; }
        /// <summary>
        /// 垂直缩放模式
        /// </summary>
        ExpandMode VerticalalExpandMode { get; set; }
    }
}
