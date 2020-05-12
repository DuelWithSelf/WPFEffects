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
    public class NormalMenu : IconMenu
    {
        public string Text
        {
            get { return (string)base.GetValue(TextProperty); }
            set { base.SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(NormalMenu),
                new FrameworkPropertyMetadata(""));

        public Thickness TextMargin
        {
            get { return (Thickness)base.GetValue(TextMarginProperty); }
            set { base.SetValue(TextMarginProperty, value); }
        }
        public static readonly DependencyProperty TextMarginProperty =
            DependencyProperty.Register("TextMargin", typeof(Thickness), typeof(NormalMenu),
                new FrameworkPropertyMetadata(new Thickness(5, 0, 0, 0)));

        static NormalMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NormalMenu), new FrameworkPropertyMetadata(typeof(NormalMenu)));
        }
    }
}
