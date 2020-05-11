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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFEffects.Core.CommonFrms;

namespace WPFEffects.Modules.Carousel
{
    /// <summary>
    /// CarouselModuleView.xaml 的交互逻辑
    /// </summary>
    public partial class CarouselModuleView : BaseModuleView
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

        public CarouselModuleView()
        {
            InitializeComponent();
            this.LoadImgList();
            this.Loaded += CarouselModuleView_Loaded;
            this.Unloaded += CarouselModuleView_Unloaded;
        }

        private void CarouselModuleView_Unloaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= new EventHandler(CompositionTarget_Rendering);
        }

        private void CarouselModuleView_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= CarouselModuleView_Loaded;

            this.CreateElements();

            this.GdRoot.MouseLeftButtonDown += GdRoot_MouseLeftButtonDown;
            this.MouseMove += Carousel2DView_MouseMove;
            this.MouseUp += Carousel2DView_MouseUp;
        }

        #region Create Elements

        private double Radius = 325d;
        private double VisualCount = 10d;
        private List<AnimImage> ElementList;
        private double CenterDegree = 180d;
        private double TotalDegree = 0;
        private double ElementWidth = 260;
        private double ElementHeight = 180;

        private double GetScaledSize(double degrees)
        {
            return GetCoefficient(degrees);
        }

        private double GetCoefficient(double degrees)
        {
            return 1.0 - Math.Cos(ConvertToRads(degrees)) / 2.0 - 0.5;
        }

        private double ConvertToRads(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        private int GetZValue(double degrees)
        {
            return (int)((360 * GetCoefficient(degrees)) * 1000);
        }

        private void CreateElements()
        {
            double dAverageDegree = 360d / VisualCount;
            this.TotalDegree = this.FileItems.Count * dAverageDegree;

            this.ElementList = new List<AnimImage>();
            for (int i = 0; i < this.FileItems.Count; i++)
            {
                string sFile = this.FileItems[i];
                AnimImage oItem = new AnimImage(sFile);
                oItem.MouseLeftButtonDown += OItem_MouseLeftButtonDown;
                oItem.MouseLeftButtonUp += OItem_MouseLeftButtonUp;
                oItem.Width = this.ElementWidth;
                oItem.Height = this.ElementHeight;
                oItem.Y = 175d;
                oItem.Degree = i * dAverageDegree;
                this.ElementList.Add(oItem);
            }

            this.UpdateLocation();
        }

        private AnimImage CurNavItem;

        private void OItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.IsMouseDown && CurNavItem == sender && this.TotalMoveDegree < 50)
            {
                this.InertiaDegree = CenterDegree - this.CurNavItem.Degree;
                this.CurNavItem = null;
                this.IsMouseDown = false;
                if (this.InertiaDegree != 0)
                    CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
                e.Handled = true;
            }
        }

        private void OItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CurNavItem = sender as AnimImage;
        }

        private void UpdateLocation()
        {
            for (int i = 0; i < this.ElementList.Count; i++)
            {
                AnimImage oItem = this.ElementList[i];

                if (oItem.Degree - this.CenterDegree >= this.TotalDegree / 2d)
                    oItem.Degree -= this.TotalDegree;
                else if (this.CenterDegree - oItem.Degree > this.TotalDegree / 2d)
                    oItem.Degree += this.TotalDegree;

                if (oItem.Degree >= 90d && oItem.Degree < 270d) // Degree 在90-270之间的显示
                    this.SetElementVisiable(oItem);
                else
                    this.SetElementInvisiable(oItem);
            }
        }

        private void SetElementVisiable(AnimImage oItem)
        {
            if (oItem == null)
                return;

            if (!oItem.IsVisible)
            {
                if (!this.CvMain.Children.Contains(oItem))
                {
                    oItem.IsVisible = true;
                    this.CvMain.Children.Add(oItem);
                }
            }

            this.DoUpdateElementLocation(oItem);
        }

        private void SetElementInvisiable(AnimImage oItem)
        {
            if (oItem.IsVisible)
            {
                if (this.CvMain.Children.Contains(oItem))
                {
                    this.CvMain.Children.Remove(oItem);
                    oItem.IsVisible = false;
                }
            }
        }

        public void DoUpdateElementLocation(AnimImage oItem)
        {
            double CenterX = this.GdRoot.ActualWidth / 2.0;
            double dX = -Radius * Math.Sin(ConvertToRads(oItem.Degree));
            oItem.X = (dX + CenterX - this.ElementWidth / 2d);
            double dScale = GetScaledSize(oItem.Degree);
            oItem.ScaleX = dScale;
            oItem.ScaleY = dScale;
            int nZIndex = GetZValue(oItem.Degree);
            Canvas.SetZIndex(oItem, nZIndex);
        }

        #endregion

        #region Drag And Move

        private bool IsMouseDown = false;
        private double PreviousX = 0;
        private double CurrentX = 0;
        private double IntervalDegree = 0;
        private double InertiaDegree = 0;
        private double TotalMoveDegree = 0;

        private void GdRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.IsMouseDown = true;
            this.IntervalDegree = 0;
            this.PreviousX = e.GetPosition(this).X;
            this.TotalMoveDegree = 0;
            CompositionTarget.Rendering -= new EventHandler(CompositionTarget_Rendering);
        }

        private void Carousel2DView_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsMouseDown)
            {
                this.CurrentX = e.GetPosition(this).X;
                this.IntervalDegree = this.CurrentX - this.PreviousX;
                this.TotalMoveDegree += Math.Abs(this.IntervalDegree * 0.5d);
                this.InertiaDegree = this.IntervalDegree * 5d;

                for (int i = 0; i < this.ElementList.Count; i++)
                {
                    AnimImage oItem = this.ElementList[i];
                    oItem.Degree += this.IntervalDegree;
                }
                this.UpdateLocation();
                this.PreviousX = this.CurrentX;
            }
        }

        private void Carousel2DView_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.IsMouseDown)
            {
                this.IsMouseDown = false;
                this.CurNavItem = null;
                if (this.InertiaDegree != 0)
                {
                    CompositionTarget.Rendering -= new EventHandler(CompositionTarget_Rendering);
                    CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
                }
            }
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            double dIntervalDegree = this.InertiaDegree * 0.1;
            for (int i = 0; i < this.ElementList.Count; i++)
            {
                AnimImage oItem = this.ElementList[i];
                oItem.Degree += dIntervalDegree;
            }
            this.UpdateLocation();

            this.InertiaDegree -= dIntervalDegree;
            if (Math.Abs(this.InertiaDegree) < 0.1)
                CompositionTarget.Rendering -= new EventHandler(CompositionTarget_Rendering);
        }

        #endregion
    }
}
