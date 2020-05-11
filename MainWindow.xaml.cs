using System;
using System.Collections;
using System.Collections.Generic;
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
using WPFEffects.Core.Common;
using WPFEffects.Core.CustomFrms;
using WPFEffects.Modules;
using WPFEffects.Modules.AboutMe;
using WPFEffects.Modules.Carousel;
using WPFEffects.Modules.Chart;
using WPFEffects.Modules.Dynamic;
using WPFEffects.Modules.Effects;
using WPFEffects.Modules.Imgs;
using WPFEffects.Modules.Performance;
using WPFEffects.Modules.Preview;

namespace WPFEffects
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private BaseModuleView CreateContentView(string sKey)
        {
            if (sKey == "AboutMe")
                return new AboutMeModuleView();
            else if (sKey == "PathData")
                return new PathDataModuleView();
            else if (sKey == "TextblockEffect")
                return new TextblockEffectModuleView();
            else if (sKey == "Carousel")
                return new CarouselModuleView();
            else if (sKey == "Carousel3D")
                return new Carousel3DModuleView();
            else if (sKey == "AnimLine")
                return new AnimLineModuleView();
            else if (sKey == "HistogramChart")
                return new HistogramChartModuleView();
            else if (sKey == "ImgCoordinate")
                return new ImgCoordinateModuleView();
            else if (sKey == "ImagePerformance")
                return new ImagePerformanceModuleView();
            else if (sKey == "ImagePerformance2")
                return new ImagePerformance2ModuleView();
            else if (sKey == "PieChart")
                return new PieChartModuleView();
            else if (sKey == "RadianChart")
                return new RadianChartModuleView();
            else if (sKey == "Advertise")
                return new AdvertiseModuleView();

            return new AboutMeModuleView();
        }

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AsynchUtils.Dispose();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.RectHeader.MouseDown += RectHeader_MouseDown;
            this.MenuShutdown.MouseLeftButtonUp += MenuShutdown_MouseLeftButtonUp;

            for(int i = 0;i< this.SpNavItems.Children.Count; i++)
            {
                ListMenuBox menuBox = this.SpNavItems.Children[i] as ListMenuBox;
                if(menuBox != null)
                {
                    menuBox.OnKeySelected += MenuBox_OnKeySelected;
                }
            }

            this.MenuCloseAllNavHeader.MouseLeftButtonUp += MenuCloseAllNavHeader_MouseLeftButtonUp;
            this.MenuLeft.MouseLeftButtonUp += MenuLeft_MouseLeftButtonUp;
            this.MenuRight.MouseLeftButtonUp += MenuRight_MouseLeftButtonUp;
            this.UpdateNavHeaderStatus();
        }

        private void MenuCloseAllNavHeader_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.HsPages.Clear();
            this.SpNavHeaders.Children.Clear();
            this.GdContent.Children.Clear();
            this.UpdateListMenuBoxSelectStatus("");
            this.UpdateNavHeaderStatus();
        }

        private Hashtable HsPages;

        private void MenuBox_OnKeySelected(ListMenuBox sender, string sText, string sKey)
        {
            // 取消其他ListMenuBox的选中状态
            for (int i = 0; i < this.SpNavItems.Children.Count; i++)
            {
                ListMenuBox menuBox = this.SpNavItems.Children[i] as ListMenuBox;
                if (menuBox != null && menuBox != sender)
                {
                    menuBox.CancelSelectAll();
                }
            }

            // 创建界面
            if (this.HsPages == null)
                this.HsPages = new Hashtable();

            this.GdContent.Children.Clear();
            if (this.HsPages.Contains(sKey))
            {
                // Focus To NavHeader
                for(int i=0; i < this.SpNavHeaders.Children.Count; i++)
                {
                    NavHeader navHeader = this.SpNavHeaders.Children[i] as NavHeader;
                    if (navHeader != null)
                    {
                        if (navHeader.Key != sKey)
                            navHeader.IsSelected = false;
                        else
                            navHeader.IsSelected = true;
                    }
                }

                // Move NavHeader Container keep current navheader visible
                this.UpdateNavHeaderStatus();

                BaseModuleView moduleView = this.HsPages[sKey] as BaseModuleView;
                if (moduleView != null)
                    this.GdContent.Children.Add(moduleView);
            }
            else
            {
                // 取消顶部导览按钮选中状态
                for (int i = 0; i < this.SpNavHeaders.Children.Count; i++)
                {
                    NavHeader navHeader = this.SpNavHeaders.Children[i] as NavHeader;
                    if(navHeader != null)
                        navHeader.IsSelected = false;
                }

                // 创建顶部导览按钮
                NavHeader newNavHeader = new NavHeader();
                newNavHeader.Text = sText;
                newNavHeader.Key = sKey;
                newNavHeader.OnClosed += NewNavHeader_OnClosed;
                newNavHeader.OnFocused += NewNavHeader_OnFocused;
                newNavHeader.IsSelected = true;
                this.SpNavHeaders.Children.Add(newNavHeader);
                
                
                this.UpdateNavHeaderStatus();

                // 创建内容面板
                BaseModuleView moduleView = this.CreateContentView(sKey);
                this.GdContent.Children.Clear();
                this.GdContent.Children.Add(moduleView);
                this.HsPages.Add(sKey, moduleView);
            }
        }

        private void NewNavHeader_OnFocused(NavHeader nextFocus)
        {
            if (!nextFocus.IsSelected)
            {
                for (int i = 0; i < this.SpNavHeaders.Children.Count; i++)
                {
                    NavHeader navHeader = this.SpNavHeaders.Children[i] as NavHeader;
                    if(navHeader != null)
                        navHeader.IsSelected = false;
                }
                this.DoFocusOnNavHeader(nextFocus);
            }
        }

        private void NewNavHeader_OnClosed(NavHeader arg)
        {
            int nIndex = this.SpNavHeaders.Children.IndexOf(arg);
            nIndex -= 1;
            if (nIndex < 0)
                nIndex = 0;
            
            this.SpNavHeaders.Children.Remove(arg);
            this.GdContent.Children.Clear();
            this.HsPages.Remove(arg.Key);

            if(this.SpNavHeaders.Children.Count > 0)
            {
                if (arg.IsSelected == true)
                {
                    NavHeader previousNavHeader = this.SpNavHeaders.Children[nIndex] as NavHeader;
                    this.DoFocusOnNavHeader(previousNavHeader);
                }
                else
                {
                    NavHeader focusItem = this.GetFocusedNavHeader();
                    this.DoFocusOnNavHeader(focusItem);
                }
            }
            else
            {
                this.UpdateListMenuBoxSelectStatus("");
                this.UpdateNavHeaderStatus();
            }
        }

        private void DoFocusOnNavHeader(NavHeader nextFocus)
        {
            if (nextFocus != null)
            {
                nextFocus.IsSelected = true;
                this.GdContent.Children.Clear();
                this.UpdateListMenuBoxSelectStatus(nextFocus.Key);
                this.UpdateNavHeaderStatus();
                BaseModuleView moduleView = this.HsPages[nextFocus.Key] as BaseModuleView;
                if (moduleView != null)
                    this.GdContent.Children.Add(moduleView);
            }
        }

        private void UpdateListMenuBoxSelectStatus(string sKey)
        {
            for (int i = 0; i < this.SpNavItems.Children.Count; i++)
            {
                ListMenuBox menuBox = this.SpNavItems.Children[i] as ListMenuBox;
                if (menuBox != null)
                {
                    if (menuBox.Items.Count == 0)
                    {
                        if (menuBox.Key == sKey)
                            menuBox.IsExpanded = true;
                        else
                            menuBox.IsExpanded = false;
                    }
                    else
                    {
                        bool bIsChildSelect = false;
                        for (int j = 0; j < menuBox.Items.Count; j++)
                        {
                            ListMenuItem menuItem = menuBox.Items[j] as ListMenuItem;
                            if (menuItem != null)
                            {
                                if (menuItem.Key != sKey)
                                    menuItem.IsSelected = false;
                                else
                                {
                                    menuItem.IsSelected = true;
                                    bIsChildSelect = true;
                                }
                            }
                        }

                        if(!bIsChildSelect)
                            menuBox.IsExpanded = false;
                        else
                            menuBox.IsExpanded = true;
                    }
                }
            }
        }

        private async void DoUpdateAvailable()
        {
            await Task.Delay(100);

            if (this.SpNavHeaders.Children.Count > 0)
                this.MenuCloseAllNavHeader.IsEnabled = true;
            else
            {
                this.MenuLeft.IsEnabled = false;
                this.MenuRight.IsEnabled = false;
                this.MenuCloseAllNavHeader.IsEnabled = false;
                return;
            }

            double dLeft = Canvas.GetLeft(this.SpNavHeaders);
            if (dLeft < 0)
                this.MenuLeft.IsEnabled = true;
            else
                this.MenuLeft.IsEnabled = false;

            if (dLeft + this.SpNavHeaders.ActualWidth > this.CvNavHeaders.ActualWidth)
                this.MenuRight.IsEnabled = true;
            else
                this.MenuRight.IsEnabled = false;
        }

        private NavHeader GetFocusedNavHeader()
        {
            for(int i=0; i< this.SpNavHeaders.Children.Count;i++)
            {
                NavHeader navItem = this.SpNavHeaders.Children[i] as NavHeader;
                if (navItem != null && navItem.IsSelected)
                    return navItem;
            }
            return null;
        }

        private void UpdateNavHeaderStatus()
        {
            this.UpdateLayout();
            this.DoUpdateAvailable();

            NavHeader navFocus = this.GetFocusedNavHeader();
            if(navFocus != null)
            {
                Point posToCvNavHeader = navFocus.TranslatePoint(new Point(0, 0), this.CvNavHeaders);
                double dMoveDistance = 0;
                if (posToCvNavHeader.X < 0)
                    dMoveDistance = -posToCvNavHeader.X;
                if (posToCvNavHeader.X > this.CvNavHeaders.ActualWidth - navFocus.ActualWidth)
                    dMoveDistance = this.CvNavHeaders.ActualWidth - navFocus.ActualWidth - posToCvNavHeader.X;

                double dLeft = Canvas.GetLeft(this.SpNavHeaders);
                Canvas.SetLeft(this.SpNavHeaders, dLeft + dMoveDistance);
            }
        }

        private void MenuRight_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double dMinX = 1000;
            NavHeader minXNavItem = null;
            for (int i = 0; i < this.SpNavHeaders.Children.Count; i++)
            {
                NavHeader navItem = this.SpNavHeaders.Children[i] as NavHeader;
                if (navItem != null)
                {
                    Point posToCvNavHeader = navItem.TranslatePoint(new Point(0, 0), this.CvNavHeaders);
                    if (posToCvNavHeader.X > this.CvNavHeaders.ActualWidth - navItem.ActualWidth)
                    {
                        if (posToCvNavHeader.X < dMinX)
                        {
                            dMinX = posToCvNavHeader.X;
                            minXNavItem = navItem;
                        }
                    }
                }
            }

            if (minXNavItem != null)
            {
                double dMoveDistance = this.CvNavHeaders.ActualWidth - minXNavItem.ActualWidth - dMinX;
                double dLeft = Canvas.GetLeft(this.SpNavHeaders);
                Canvas.SetLeft(this.SpNavHeaders, dLeft + dMoveDistance);
                this.DoUpdateAvailable();
            }
        }

        private void MenuLeft_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double dMaxX = -1000;
            for (int i = 0; i < this.SpNavHeaders.Children.Count; i++)
            {
                NavHeader navItem = this.SpNavHeaders.Children[i] as NavHeader;
                if (navItem != null)
                {
                    Point posToCvNavHeader = navItem.TranslatePoint(new Point(0, 0), this.CvNavHeaders);
                    if(posToCvNavHeader.X < 0)
                    {
                        if(posToCvNavHeader.X + navItem.ActualWidth < 0)
                        {
                            if (posToCvNavHeader.X > dMaxX)
                                dMaxX = posToCvNavHeader.X;
                        }
                        else
                        {
                            if (i == 0)
                            {
                                if (posToCvNavHeader.X > dMaxX)
                                    dMaxX = posToCvNavHeader.X;
                            }
                        }
                    }
                }
            }

            double dLeft = Canvas.GetLeft(this.SpNavHeaders);
            Canvas.SetLeft(this.SpNavHeaders, dLeft - dMaxX);
            this.DoUpdateAvailable();
        }

        private void MenuShutdown_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void RectHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    DragMove();
            }
        }
    }
}
