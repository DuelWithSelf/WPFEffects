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
using WPFEffects.Core.Common;

namespace WPFEffects.Core.CustomFrms
{
    [TemplatePart(Name = Part_BdrClose, Type = typeof(Border))]
    public class NavHeader : Control
    {
        private const string Part_BdrClose = "Part_BdrClose";
        private Border MenuClose;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            MenuClose = GetTemplateChild(Part_BdrClose) as Border;
        }

        public NavHeader() 
        {
            this.Loaded += NavHeader_Loaded;
        }

        private void NavHeader_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= NavHeader_Loaded;

            if(this.MenuClose != null)
                this.MenuClose.MouseLeftButtonUp += MenuClose_MouseLeftButtonUp;
            this.MouseLeftButtonUp += NavHeader_MouseLeftButtonUp;
        }

        public event DelegateMethod<NavHeader> OnFocused;

        private void NavHeader_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.OnFocused != null)
                this.OnFocused(this);
        }

        public event DelegateMethod<NavHeader> OnClosed;

        private void MenuClose_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.OnClosed != null)
                this.OnClosed(this);
            e.Handled = true;
        }

        public bool IsSelected
        {
            get { return (bool)base.GetValue(IsSelectedProperty); }
            set { base.SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(NavHeader),
                new FrameworkPropertyMetadata(false));

        public string Key
        {
            get { return (string)base.GetValue(KeyProperty); }
            set { base.SetValue(KeyProperty, value); }
        }
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register("Key", typeof(string), typeof(NavHeader),
                new FrameworkPropertyMetadata(""));

        public string Text
        {
            get { return (string)base.GetValue(TextProperty); }
            set { base.SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(NavHeader),
                new FrameworkPropertyMetadata(""));

        static NavHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavHeader), new FrameworkPropertyMetadata(typeof(NavHeader)));
        }
    }
}
