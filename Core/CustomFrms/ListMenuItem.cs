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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFEffects.Core.CustomFrms
{
      public class ListMenuItem : Control
    {
        public string IconData
        {
            get { return (string)base.GetValue(IconDataProperty); }
            set { base.SetValue(IconDataProperty, value); }
        }
        public static readonly DependencyProperty IconDataProperty =
            DependencyProperty.Register("IconData", typeof(string), typeof(ListMenuItem),
                new FrameworkPropertyMetadata(""));

        public double IconWidth
        {
            get { return (double)base.GetValue(IconWidthProperty); }
            set { base.SetValue(IconWidthProperty, value); }
        }
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register("IconWidth", typeof(double), typeof(ListMenuItem),
                new FrameworkPropertyMetadata(60d));

        public double IconHeight
        {
            get { return (double)base.GetValue(IconHeightProperty); }
            set { base.SetValue(IconHeightProperty, value); }
        }
        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register("IconHeight", typeof(double), typeof(ListMenuItem),
                new FrameworkPropertyMetadata(60d));

        public Brush NormalBrush
        {
            get { return (Brush)base.GetValue(NormalBrushProperty); }
            set { base.SetValue(NormalBrushProperty, value); }
        }
        public static readonly DependencyProperty NormalBrushProperty =
            DependencyProperty.Register("NormalBrush", typeof(Brush), typeof(ListMenuItem),
                new FrameworkPropertyMetadata(Brushes.White));

        public Brush FocusBrush
        {
            get { return (Brush)base.GetValue(FocusBrushProperty); }
            set { base.SetValue(FocusBrushProperty, value); }
        }
        public static readonly DependencyProperty FocusBrushProperty =
            DependencyProperty.Register("FocusBrush", typeof(Brush), typeof(ListMenuItem),
                new FrameworkPropertyMetadata(Brushes.Blue));

        public Brush FocusBackground
        {
            get { return (Brush)base.GetValue(FocusBackgroundProperty); }
            set { base.SetValue(FocusBackgroundProperty, value); }
        }
        public static readonly DependencyProperty FocusBackgroundProperty =
            DependencyProperty.Register("FocusBackground", typeof(Brush), typeof(ListMenuItem),
                new FrameworkPropertyMetadata(Brushes.Transparent));

        public bool IsSelected
        {
            get { return (bool)base.GetValue(IsSelectedProperty); }
            set { base.SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(ListMenuItem),
                new FrameworkPropertyMetadata(false));

        public string Text
        {
            get { return (string)base.GetValue(TextProperty); }
            set { base.SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ListMenuItem),
                new FrameworkPropertyMetadata(""));

        public string Key
        {
            get { return (string)base.GetValue(KeyProperty); }
            set { base.SetValue(KeyProperty, value); }
        }
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register("Key", typeof(string), typeof(ListMenuItem),
                new FrameworkPropertyMetadata(""));

        public Thickness TextPadding
        {
            get { return (Thickness)base.GetValue(TextPaddingProperty); }
            set { base.SetValue(TextPaddingProperty, value); }
        }
        public static readonly DependencyProperty TextPaddingProperty =
            DependencyProperty.Register("TextPadding", typeof(Thickness), typeof(ListMenuItem),
                new FrameworkPropertyMetadata(new Thickness(0, 0, 0, 0)));

        static ListMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListMenuItem), new FrameworkPropertyMetadata(typeof(ListMenuItem)));
        }
    }
}
