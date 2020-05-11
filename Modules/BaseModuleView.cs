using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WPFEffects.Modules
{
    public class BaseModuleView: UserControl
    {
        private TranslateTransform CurTranslate;
        private DoubleAnimation XAnim;
        private DoubleAnimation OpacityAnim;
        private double OriginX = 50;
        public BaseModuleView()
        {
            this.DoInitAnim();
            this.Loaded += BaseModuleView_Loaded;
            this.Unloaded += BaseModuleView_Unloaded;
        }

        private void DoInitAnim() {
            this.CurTranslate = new TranslateTransform();
            this.CurTranslate.X = this.OriginX;
            this.RenderTransform = this.CurTranslate;
            this.Opacity = 0;
            XAnim = new DoubleAnimation(0, TimeSpan.FromSeconds(0.3));
            XAnim.EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut };
            OpacityAnim = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
        }

        private void DoLoadAnim() {
            this.CurTranslate.BeginAnimation(TranslateTransform.XProperty, XAnim);
            this.BeginAnimation(UIElement.OpacityProperty, OpacityAnim);
        }

        private void DoUnloadAnim()
        {
            this.CurTranslate.BeginAnimation(TranslateTransform.XProperty, null);
            this.BeginAnimation(UIElement.OpacityProperty, null);

            this.CurTranslate.X = this.OriginX;
            this.Opacity = 0;
        }

        private void BaseModuleView_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DoUnloadAnim();
        }

        private void BaseModuleView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DoLoadAnim();
        }
    }
}
