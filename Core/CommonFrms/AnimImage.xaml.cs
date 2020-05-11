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
using WPFEffects.Core.Common;

namespace WPFEffects.Core.CommonFrms
{
    /// <summary>
    /// AnimImage.xaml 的交互逻辑
    /// </summary>
    public partial class AnimImage : UserControl
    {
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(AnimImage), new UIPropertyMetadata(0.0));

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(AnimImage), new UIPropertyMetadata(0.0));

        public double ScaleX
        {
            get { return (double)GetValue(ScaleXProperty); }
            set { SetValue(ScaleXProperty, value); }
        }
        public static readonly DependencyProperty ScaleXProperty =
            DependencyProperty.Register("ScaleX", typeof(double), typeof(AnimImage), new UIPropertyMetadata(1.0));

        public double ScaleY
        {
            get { return (double)GetValue(ScaleYProperty); }
            set { SetValue(ScaleYProperty, value); }
        }
        public static readonly DependencyProperty ScaleYProperty =
            DependencyProperty.Register("ScaleY", typeof(double), typeof(AnimImage), new UIPropertyMetadata(1.0));

        public double Degree;
        private string FileSrc = "";

        private bool _IsVisible = false;
        public new bool IsVisible
        {
            get { return _IsVisible; }
            set
            {
                _IsVisible = value;
                if (value)
                    this.LoadUiImmediate();
            }
        }

        private bool IsUiLoaded = false;

        public void LoadUiImmediate()
        {
            if (!IsUiLoaded)
            {
                IsUiLoaded = true;
                try
                {
                    if (File.Exists(FileSrc))
                        this.ImgMain.Source = new BitmapImage(new Uri(FileSrc));
                }
                catch { }

                //Console.WriteLine(DateTime.Now.ToLongTimeString());
            }
        }

        public AnimImage(string sFile)
        {
            InitializeComponent();
            this.FileSrc = sFile;
            string sFileName = System.IO.Path.GetFileNameWithoutExtension(sFile);
            this.TbkTitle.Text = sFileName;
            this.Loaded += ImageItem_Loaded;
            this.DataContext = this;
        }

        private void ImageItem_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= ImageItem_Loaded;
            AsynchUtils.AsynchSleepExecuteFunc(this.Dispatcher, LoadUiImmediate, 0.5);
        }

        public void Dispose()
        {
            this.ImgMain.Source = null;
        }
    }
}
