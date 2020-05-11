using System;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFEffects.Modules.AboutMe
{
    /// <summary>
    /// AdvertiseModuleView.xaml 的交互逻辑
    /// </summary>
    public partial class AdvertiseModuleView : BaseModuleView
    {
        public SolidColorBrush NeonBrush
        {
            get { return (SolidColorBrush)base.GetValue(NeonBrushProperty); }
            set { base.SetValue(NeonBrushProperty, value); }
        }
        public static readonly DependencyProperty NeonBrushProperty =
            DependencyProperty.Register("NeonBrush", typeof(SolidColorBrush), typeof(AdvertiseModuleView),
                new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(78, 201, 250))));

        public Color NeonColor
        {
            get { return (Color)base.GetValue(NeonColorProperty); }
            set { base.SetValue(NeonColorProperty, value); }
        }
        public static readonly DependencyProperty NeonColorProperty =
            DependencyProperty.Register("NeonColor", typeof(Color), typeof(AdvertiseModuleView),
                new FrameworkPropertyMetadata(Color.FromRgb(78, 201, 250), OnNeonColorChanged));
        private static void OnNeonColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            AdvertiseModuleView advertiseModuleView = sender as AdvertiseModuleView;
            if (sender != null)
                advertiseModuleView.NeonBrush = new SolidColorBrush((Color)e.NewValue);
        }

        private void CreateColorAnim()
        {
            ColorAnimationUsingKeyFrames colorKeyFrms = new ColorAnimationUsingKeyFrames();
            colorKeyFrms.KeyFrames.Add(new EasingColorKeyFrame(Color.FromRgb(245, 104, 5),
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5d))));
            colorKeyFrms.KeyFrames.Add(new EasingColorKeyFrame(Color.FromRgb(236, 247, 8),
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1d))));
            colorKeyFrms.KeyFrames.Add(new EasingColorKeyFrame(Color.FromRgb(10, 124, 238),
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1.5d))));
            colorKeyFrms.KeyFrames.Add(new EasingColorKeyFrame(Color.FromRgb(69, 205, 199),
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2d))));
            colorKeyFrms.KeyFrames.Add(new EasingColorKeyFrame(Color.FromRgb(8, 83, 158),
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2.5d))));
            colorKeyFrms.KeyFrames.Add(new EasingColorKeyFrame(Color.FromRgb(234, 112, 112),
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(3d))));
            colorKeyFrms.KeyFrames.Add(new EasingColorKeyFrame(Color.FromRgb(6, 247, 203),
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(3.5d))));
            colorKeyFrms.KeyFrames.Add(new EasingColorKeyFrame(Color.FromRgb(78, 201, 250),
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(4d))));

            colorKeyFrms.RepeatBehavior = RepeatBehavior.Forever;
            this.BeginAnimation(NeonColorProperty, colorKeyFrms);
        }

        public AdvertiseModuleView()
        {
            InitializeComponent();

            this.DataContext = this;
            this.Loaded += AdvertiseModuleView_Loaded;
            this.Unloaded += AdvertiseModuleView_Unloaded;
        }

        private void AdvertiseModuleView_Unloaded(object sender, RoutedEventArgs e)
        {
            this.BeginAnimation(NeonColorProperty, null);
        }

        private void AdvertiseModuleView_Loaded(object sender, RoutedEventArgs e)
        {
            this.CreateColorAnim();
        }
    }
}
