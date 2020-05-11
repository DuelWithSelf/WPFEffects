using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using WPFEffects.Core.Common;

namespace WPFEffects.Core.CustomFrms
{
    [TemplatePart(Name = Part_ItemsContainer, Type = typeof(StackPanel))]
    [TemplatePart(Name = Part_Arrow, Type = typeof(Path))]
    public class ListMenuBox : ItemsControl
    {
        private const string Part_ItemsContainer = "Part_ItemsContainer";
        private StackPanel ItemsContainer;
        private const string Part_Arrow = "Part_Arrow";
        private Path PathArrow;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PathArrow = GetTemplateChild(Part_Arrow) as Path;
            ItemsContainer = GetTemplateChild(Part_ItemsContainer) as StackPanel;
        }

        public string IconData
        {
            get { return (string)base.GetValue(IconDataProperty); }
            set { base.SetValue(IconDataProperty, value); }
        }
        public static readonly DependencyProperty IconDataProperty =
            DependencyProperty.Register("IconData", typeof(string), typeof(ListMenuBox),
                new FrameworkPropertyMetadata(""));

        public double IconWidth
        {
            get { return (double)base.GetValue(IconWidthProperty); }
            set { base.SetValue(IconWidthProperty, value); }
        }
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register("IconWidth", typeof(double), typeof(ListMenuBox),
                new FrameworkPropertyMetadata(60d));

        public double IconHeight
        {
            get { return (double)base.GetValue(IconHeightProperty); }
            set { base.SetValue(IconHeightProperty, value); }
        }
        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register("IconHeight", typeof(double), typeof(ListMenuBox),
                new FrameworkPropertyMetadata(60d));

        public Brush NormalBrush
        {
            get { return (Brush)base.GetValue(NormalBrushProperty); }
            set { base.SetValue(NormalBrushProperty, value); }
        }
        public static readonly DependencyProperty NormalBrushProperty =
            DependencyProperty.Register("NormalBrush", typeof(Brush), typeof(ListMenuBox),
                new FrameworkPropertyMetadata(Brushes.White));

        public Brush FocusBrush
        {
            get { return (Brush)base.GetValue(FocusBrushProperty); }
            set { base.SetValue(FocusBrushProperty, value); }
        }
        public static readonly DependencyProperty FocusBrushProperty =
            DependencyProperty.Register("FocusBrush", typeof(Brush), typeof(ListMenuBox),
                new FrameworkPropertyMetadata(Brushes.Blue));

        public Brush FocusBackground
        {
            get { return (Brush)base.GetValue(FocusBackgroundProperty); }
            set { base.SetValue(FocusBackgroundProperty, value); }
        }
        public static readonly DependencyProperty FocusBackgroundProperty =
            DependencyProperty.Register("FocusBackground", typeof(Brush), typeof(ListMenuBox),
                new FrameworkPropertyMetadata(Brushes.Transparent));

        public bool IsExpanded
        {
            get { return (bool)base.GetValue(IsExpandedProperty); }
            set { base.SetValue(IsExpandedProperty, value); }
        }
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(ListMenuBox),
                new FrameworkPropertyMetadata(false,OnSelectedChanged));

        private static void OnSelectedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ListMenuBox menuGroup = obj as ListMenuBox;
            if (menuGroup.PathArrow != null)
            {
                // 判断Arrow的旋转属性是否被冻结
                Path pathArrow = menuGroup.PathArrow;
                TransformGroup transGroup = pathArrow.RenderTransform as TransformGroup;
                RotateTransform rotate = transGroup.Children[0] as RotateTransform;
                if(rotate.IsFrozen)
                {
                    pathArrow.RenderTransformOrigin = new Point(0.5, 0.5);
                    transGroup = new TransformGroup();
                    transGroup.Children.Add(new RotateTransform());
                    rotate = transGroup.Children[0] as RotateTransform;
                    pathArrow.RenderTransform = transGroup;
                }

                if ((bool)e.NewValue)
                {
                    // 右箭头下旋动画
                    DoubleAnimation animArrow = new DoubleAnimation(90, TimeSpan.FromSeconds(0.2));
                    rotate.BeginAnimation(RotateTransform.AngleProperty, animArrow);

                    // 子集收回动画
                    int nChildrenCount = menuGroup.Items.Count;
                    if(nChildrenCount > 0)
                    {
                        FrameworkElement item = menuGroup.Items[0] as FrameworkElement;
                        double dItemHeight = item.ActualHeight;
                        double dTotalChildHeight = nChildrenCount * dItemHeight;

                        DoubleAnimation animItemContainer = new DoubleAnimation(dTotalChildHeight, TimeSpan.FromSeconds(0.2));
                        menuGroup.ItemsContainer.BeginAnimation(FrameworkElement.HeightProperty, animItemContainer);
                    }
                }
                else
                {
                    // 右箭头收回动画
                    DoubleAnimation animArrow = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                    rotate.BeginAnimation(RotateTransform.AngleProperty, animArrow);

                    // 子集收回动画
                    int nChildrenCount = menuGroup.Items.Count;
                    if (nChildrenCount > 0)
                    {
                        DoubleAnimation animItemContainer = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                        menuGroup.ItemsContainer.BeginAnimation(FrameworkElement.HeightProperty, animItemContainer);
                    }
                }
            }
        }

        public string Text
        {
            get { return (string)base.GetValue(TextProperty); }
            set { base.SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ListMenuBox),
                new FrameworkPropertyMetadata(""));

        public string Key
        {
            get { return (string)base.GetValue(KeyProperty); }
            set { base.SetValue(KeyProperty, value); }
        }
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register("Key", typeof(string), typeof(ListMenuBox),
                new FrameworkPropertyMetadata(""));

        public Thickness TextPadding
        {
            get { return (Thickness)base.GetValue(TextPaddingProperty); }
            set { base.SetValue(TextPaddingProperty, value); }
        }
        public static readonly DependencyProperty TextPaddingProperty =
            DependencyProperty.Register("TextPadding", typeof(Thickness), typeof(ListMenuBox),
                new FrameworkPropertyMetadata(new Thickness(0, 0, 0, 0)));

        static ListMenuBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListMenuBox), 
                new FrameworkPropertyMetadata(typeof(ListMenuBox)));
        }

        public ListMenuBox()
        {
            this.MouseLeftButtonUp += ListMenuBox_MouseLeftButtonUp;
        }

        private void ListMenuBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.Items.Count == 0)
            {
                if (!this.IsExpanded)
                {
                    this.IsExpanded = true;
                    if (this.OnKeySelected != null)
                        this.OnKeySelected(this, this.Text, this.Key);
                }
            }
            else
            {
                if (!this.IsExpanded)
                    this.IsExpanded = true;
                else
                    this.IsExpanded = false;
            }
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            for(int i=0; i< this.Items.Count; i++)
            {
                if(this.Items[i] is ListMenuItem)
                {
                    ListMenuItem menuItem = this.Items[i] as ListMenuItem;
                    menuItem.MouseLeftButtonUp -= MenuItem_MouseLeftButtonUp;
                    menuItem.MouseLeftButtonUp += MenuItem_MouseLeftButtonUp;
                }
                else
                {
                    UIElement itemContent = this.Items[i] as UIElement;
                    if(itemContent != null)
                    {
                        itemContent.MouseLeftButtonUp -= ItemContent_MouseLeftButtonUp;
                        itemContent.MouseLeftButtonUp += ItemContent_MouseLeftButtonUp;
                    }
                }
            }

        }

        public event DelegateMethod<ListMenuBox, string, string> OnKeySelected;

        private void ItemContent_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void CancelSelectAllItems()
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i] is ListMenuItem)
                {
                    ListMenuItem menuItem = this.Items[i] as ListMenuItem;
                    menuItem.IsSelected = false;
                }
            }
        }

        public void CancelSelectAll()
        {
            this.CancelSelectAllItems();
            this.IsExpanded = false;
        }

        private void MenuItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.CancelSelectAllItems();
            ListMenuItem menuItem = sender as ListMenuItem;
            menuItem.IsSelected = true;
            if (this.OnKeySelected != null)
                this.OnKeySelected(this, menuItem.Text, menuItem.Key);
            e.Handled = true;
        }
    }
}
