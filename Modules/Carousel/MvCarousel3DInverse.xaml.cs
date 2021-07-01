using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPFEffects.Modules.Carousel
{
    /// <summary>
    /// MvCarousel3DInverse.xaml 的交互逻辑
    /// </summary>
    public partial class MvCarousel3DInverse : BaseModuleView
    {
        private List<string> FileItems;

        private void LoadImgList()
        {
            this.FileItems = new List<string>();
            string sPath = AppDomain.CurrentDomain.BaseDirectory + "Imgs";
            DirectoryInfo dir = new DirectoryInfo(sPath);
            FileInfo[] fis = dir.GetFiles();
            if (fis.Length > 0)
            {
                for (int j = 0; j < fis.Length; j++)
                    this.FileItems.Add(fis[j].FullName);
            }
        }

        public MvCarousel3DInverse()
        {
            InitializeComponent();
            this.LoadImgList();
            this.CreateElements();
            this.AttachEventHandlers();
            this.Loaded += Carousel3DModuleView_Loaded;
            this.Unloaded += Carousel3DModuleView_Unloaded;
        }

        private void Carousel3DModuleView_Unloaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
            CompositionTarget.Rendering -= CompositionTarget_AutoCarousel;
            this.FreeTimeListener();
        }

        private void Carousel3DModuleView_Loaded(object sender, RoutedEventArgs e)
        {
            this.CreateTimerListener();
        }

        private void AttachEventHandlers()
        {
            this.RectOverZm.MouseLeftButtonDown += RectOverZm_MouseLeftButtonDown;
            this.RectOverZm.MouseMove += RectOverZm_MouseMove;
            this.RectOverZm.MouseUp += RectOverZm_MouseUp;
        }

        #region Create Elements

        List<InteractivePanel3D> ElementList = new List<InteractivePanel3D>();

        private double MinViewportAngle = 0;
        private double MaxViewportAngle = 0;
        private double TotalDegree = 0;
        private int TemplateCount = 12;
        internal const double Radius = 14.8;
        internal const double CBoundDegree = 60;
        internal const double COriginViewprotAngel = -90;

        private bool IsEnoughCycle()
        {
            return this.FileItems.Count > this.TemplateCount / 2d;
        }

        private void CreateElements()
        {
            bool bIsNodeCountLarge = this.IsEnoughCycle();
            if (!bIsNodeCountLarge)
            {
                this.MinViewportAngle = COriginViewprotAngel;
                this.MaxViewportAngle = COriginViewprotAngel + this.FileItems.Count * (360d / this.TemplateCount);
            }

            double dAverageAngle = 360d / TemplateCount;
            this.TotalDegree = this.FileItems.Count * dAverageAngle;
            for (int nIndex = 0; nIndex < this.FileItems.Count; nIndex++)
            {
                InteractivePanel3D oVisualItem = new InteractivePanel3D(this.FileItems[nIndex], nIndex);
                double dDegree = nIndex * dAverageAngle;
                Transform3DGroup oTransform3DGroup = this.CreateVisualItemTransform(dDegree);
                oVisualItem.Transform = oTransform3DGroup;
                oVisualItem.Degree = dDegree;
                this.ElementList.Add(oVisualItem);
            }

            DoUpdateLayout();
            CompositionTarget.Rendering -= CompositionTarget_AutoCarousel;
            CompositionTarget.Rendering += CompositionTarget_AutoCarousel;
        }

        private void DoUpdateLayout()
        {
            for (int i = 0; i < ElementList.Count; i++)
            {
                InteractivePanel3D oVisualItem = this.ElementList[i];

                if (oVisualItem.Degree + this.CameraAngleYZm.Angle >= this.TotalDegree / 2d)
                    oVisualItem.Degree -= this.TotalDegree;
                else if (oVisualItem.Degree + this.CameraAngleYZm.Angle <= -this.TotalDegree / 2d)
                    oVisualItem.Degree += this.TotalDegree;

                //元素块角度与3D场景旋转角度的角度差; 角度差在定义的范围内则元素块显示，否则隐藏
                double dDistanceToCenter = Math.Abs(oVisualItem.Degree + this.CameraAngleYZm.Angle - COriginViewprotAngel);
                if (dDistanceToCenter <= CBoundDegree)
                    this.SetVisualItemVisible(oVisualItem);
                else
                    this.SetVisualItemInvisible(oVisualItem);
            }
        }

        private void SetVisualItemVisible(InteractivePanel3D oVisualItem)
        {
            if (!oVisualItem.IsVisible)
            {
                if (!this.Viewport3DZm.Children.Contains(oVisualItem))
                {
                    oVisualItem.IsVisible = true;
                    oVisualItem.Transform = this.CreateVisualItemTransform(oVisualItem.Degree);
                    this.Viewport3DZm.Children.Add(oVisualItem);
                }
            }
        }

        private void SetVisualItemInvisible(InteractivePanel3D oVisualItem)
        {
            if (oVisualItem.IsVisible)
            {
                if (this.Viewport3DZm.Children.Contains(oVisualItem))
                {
                    this.Viewport3DZm.Children.Remove(oVisualItem);
                    oVisualItem.IsVisible = false;
                }
            }
        }

        private Transform3DGroup CreateVisualItemTransform(double dDegree)
        {
            Transform3DGroup oTransform3DGroup = new Transform3DGroup();
            RotateTransform3D oRotateTransform3D = new RotateTransform3D();
            AxisAngleRotation3D oAxisAngleRotation3D = new AxisAngleRotation3D(new Vector3D(0, 1, 0),
                90 - dDegree);
            oRotateTransform3D.Rotation = oAxisAngleRotation3D;
            oTransform3DGroup.Children.Add(oRotateTransform3D);

            double dRadian = dDegree * Math.PI / 180;
            TranslateTransform3D oTranslateTransform3D = new TranslateTransform3D
            {
                OffsetX = Radius * Math.Cos(dRadian),
                OffsetY = -0.1,
                OffsetZ = Radius * Math.Sin(dRadian)
            };
            oTransform3DGroup.Children.Add(oTranslateTransform3D);
            return oTransform3DGroup;
        }

        private void FreeElements()
        {
            if (this.ElementList != null)
            {
                for (int i = 0; i < this.ElementList.Count; i++)
                {
                    InteractivePanel3D oVisualItem = this.ElementList[i];
                    oVisualItem.Dispose();
                }
                this.Viewport3DZm.Children.Clear();
                this.ElementList.Clear();
                this.ElementList = null;
            }
        }

        #endregion

        #region Drag And Move

        private bool IsMouseDown = false;
        private double CurrentX;
        private double PreviousX;
        private double IntervalX = 0;
        private double TargetAngle = 0;

        private void CalcCarsouelBufferAngel()
        {
            double dAngel = this.CameraAngleYZm.Angle - this.IntervalX * 0.3;
            if (!this.IsEnoughCycle())
            {
                if (dAngel < this.MinViewportAngle)
                    dAngel = this.MinViewportAngle;
                else if (dAngel > this.MaxViewportAngle)
                    dAngel = this.MaxViewportAngle;
            }

            this.TargetAngle = dAngel;
        }

        private void RectOverZm_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseDown)
            {
                this.IsMouseDown = false;

                this.CalcCarsouelBufferAngel();
                if (this.CameraAngleYZm.Angle != this.TargetAngle)
                {
                    CompositionTarget.Rendering -= CompositionTarget_Rendering;
                    CompositionTarget.Rendering += CompositionTarget_Rendering;
                }
            }
        }

        private void RectOverZm_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown)
            {
                this.CurrentX = e.GetPosition(this).X;
                this.IntervalX = this.CurrentX - this.PreviousX;
                this.CameraAngleYZm.Angle = this.CameraAngleYZm.Angle - IntervalX * 0.1;
                this.DoUpdateLayout();
                this.PreviousX = this.CurrentX;
            }
        }

        private void RectOverZm_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.IsMouseDown = true;
            this.IntervalX = 0;
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
            CompositionTarget.Rendering -= CompositionTarget_AutoCarousel;
            this.PreviousX = e.GetPosition(this).X;
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            this.CameraAngleYZm.Angle += (this.TargetAngle - this.CameraAngleYZm.Angle) * 0.1;

            if (Math.Abs(this.TargetAngle - this.CameraAngleYZm.Angle) < 0.01)
            {
                this.CameraAngleYZm.Angle = this.TargetAngle;
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
                this.TimerListener.Start();
            }
            this.DoUpdateLayout();
        }

        #endregion

        #region Auto Carousel 

        private DispatcherTimer TimerListener;

        private void CreateTimerListener()
        {
            this.TimerListener = new DispatcherTimer();
            this.TimerListener.Interval = TimeSpan.FromSeconds(5);
            this.TimerListener.Tick += TimerListener_Tick;
        }

        private void TimerListener_Tick(object sender, EventArgs e)
        {
            this.TimerListener.Stop();
            CompositionTarget.Rendering -= CompositionTarget_Rendering;

            CompositionTarget.Rendering -= CompositionTarget_AutoCarousel;
            CompositionTarget.Rendering += CompositionTarget_AutoCarousel;
        }

        private void FreeTimeListener()
        {
            if (this.TimerListener != null)
            {
                this.TimerListener.Stop();
                this.TimerListener.Tick -= this.TimerListener_Tick;
                this.TimerListener = null;
            }

        }

        private void CompositionTarget_AutoCarousel(object sender, EventArgs e)
        {
            this.CameraAngleYZm.Angle += 0.05;
            this.DoUpdateLayout();
        }

        #endregion
    }
}
