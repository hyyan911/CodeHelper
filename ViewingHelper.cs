using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace CodeHelper
{
    /// <summary>
    /// 为控件提供的图像缩放，平移操作，常用于图片查看，也可用于其他控件
    /// </summary>
    public class ViewingHelper
    {
        private double scaleFactor = 1;
        /// <summary>
        /// 当前缩放系数
        /// </summary>
        public double ScaleFactor
        {
            get { return scaleFactor; }
        }

        /// <summary>
        /// 最小缩放倍数
        /// </summary>
        public double MinScaleFactor { get; set; } = 1;

        private Point scaleCenter;
        /// <summary>
        /// 缩放中心点
        /// </summary>
        public Point ScaleCenter
        {
            get { return scaleCenter; }
        }

        private double translateXFactor = 0;
        /// <summary>
        /// 水平平移量
        /// </summary>
        public double TranslateXFactor
        {
            get { return translateXFactor; }
        }

        private double translateYFactor = 0;
        /// <summary>
        /// 垂直平移量
        /// </summary>
        public double TranslateYFactor
        {
            get { return translateYFactor; }
        }

        private Point Point = new Point();

        /// <summary>
        /// 在变换完成后触发的事件
        /// </summary>
        public event RoutedEventHandler AfterTransformUpdated = null;

        /// <summary>
        /// 缩放系数
        /// </summary>
        public double ZoomFactor { get; set; } = 1.1;

        private FrameworkElement transFormControl { get; set; } = null;
        /// <summary>
        /// 需要进行缩放的目标控件
        /// </summary>
        public FrameworkElement TransFormControl
        {
            get { return transFormControl; }
        }

        private FrameworkElement relativeControl { get; set; } = null;
        /// <summary>
        /// 参考控件，设置此值是因为在鼠标移动过程中目标控件的定位点会发生变化，因此需要再选取一个变换中保持静止的控件作为参考
        /// </summary>
        public FrameworkElement RelativeControl
        {
            get { return relativeControl; }
        }

        /// <summary>
        /// 创建托管
        /// </summary>
        /// <param name="transformControl">需要进行缩放的目标控件</param>
        /// <param name="relativeControl">参考控件，设置此值是因为在鼠标移动过程中目标控件的定位点会发生变化，因此需要再选取一个变换中保持静止的控件作为参考</param>
        /// <param name="zoomRatio"></param>
        public ViewingHelper(FrameworkElement transformControl, FrameworkElement relativeControl, double zoomRatio = 1.1)
        {
            if (transformControl == null)
            {
                throw new Exception("托管控件不能为null");
            }
            this.transFormControl = transformControl;
            this.relativeControl = relativeControl;
            ZoomFactor = zoomRatio;

            transformControl.PreviewMouseLeftButtonDown -= TransForm_MouseLeftButtonDown;
            transformControl.MouseMove -= TransForm_MouseMove;
            transformControl.MouseWheel -= TransForm_MouseWheel;
            transformControl.PreviewMouseLeftButtonUp -= TransForm_MouseLeftButtonUp;

            //添加托管
            transformControl.PreviewMouseLeftButtonDown += TransForm_MouseLeftButtonDown;
            transformControl.MouseMove += TransForm_MouseMove;
            transformControl.MouseWheel += TransForm_MouseWheel;
            transformControl.PreviewMouseLeftButtonUp += TransForm_MouseLeftButtonUp;
        }

        private void TransForm_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            transFormControl.ReleaseMouseCapture();
        }

        /// <summary>
        /// 取消托管并释放资源
        /// </summary>
        public void Dispose()
        {
            TransFormControl.PreviewMouseLeftButtonDown -= TransForm_MouseLeftButtonDown;
            TransFormControl.MouseMove -= TransForm_MouseMove;
            TransFormControl.MouseWheel -= TransForm_MouseWheel;
            TransFormControl.PreviewMouseLeftButtonUp -= TransForm_MouseLeftButtonUp;
        }

        private void Update()
        {
            TransformGroup group = new TransformGroup();
            group.Children.Add(new ScaleTransform(ScaleFactor, ScaleFactor, ScaleCenter.X, ScaleCenter.Y));
            group.Children.Add(new TranslateTransform(TranslateXFactor, TranslateYFactor));
            TransFormControl.RenderTransform = group;
            History = group;
            AfterTransformUpdated?.Invoke(this, new RoutedEventArgs());
        }

        TransformGroup History = new TransformGroup();

        /// <summary>
        /// 获取当前变换矩阵
        /// </summary>
        /// <returns></returns>
        public TransformGroup GetTramsform()
        {
            TransformGroup group = new TransformGroup();
            group.Children.Add(new ScaleTransform(ScaleFactor, ScaleFactor, ScaleCenter.X, ScaleCenter.Y));
            group.Children.Add(new TranslateTransform(TranslateXFactor, TranslateYFactor));
            return group;
        }

        /// <summary>
        /// 还原控件至原尺寸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void InitControlTransform()
        {
            if (TransFormControl == null)
            {
                throw new Exception("目标控件不能为null");
            }
            scaleFactor = 1;
            translateXFactor = 0;
            translateYFactor = 0;
            Update();
        }

        private void TransForm_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point rp = e.GetPosition(relativeControl);
            Point tp = e.GetPosition(TransFormControl);
            Point p = relativeControl.TranslatePoint(rp, TransFormControl);
            //获取当前图片位置
            scaleCenter = tp;

            if (e.Delta > 0)
                scaleFactor *= ZoomFactor;
            else
            {
                if (scaleFactor > MinScaleFactor)
                    scaleFactor /= ZoomFactor;
            }

            Update();
        }

        private void TransForm_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = e.GetPosition(RelativeControl);
                translateXFactor += p.X - Point.X;
                translateYFactor += p.Y - Point.Y;

                Point = p;

                Update();
            }
        }

        private void TransForm_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point = e.GetPosition(RelativeControl);
            TransFormControl.CaptureMouse();
        }

    }
}
