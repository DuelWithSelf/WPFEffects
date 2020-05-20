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
using System.Windows.Threading;

namespace WPFEffects.Modules.Chart.ClockChart
{
    /// <summary>
    /// ClockSlave1View.xaml 的交互逻辑
    /// </summary>
    public partial class ClockSlave1View : UserControl
    {
        public double HourAngle
        {
            get { return (double)base.GetValue(HourAngleProperty); }
            set { base.SetValue(HourAngleProperty, value); }
        }
        public static readonly DependencyProperty HourAngleProperty =
            DependencyProperty.Register("HourAngle", typeof(double), typeof(ClockSlave1View),
                new FrameworkPropertyMetadata(0d));

        public double MinuteAngle
        {
            get { return (double)base.GetValue(MinuteAngleProperty); }
            set { base.SetValue(MinuteAngleProperty, value); }
        }
        public static readonly DependencyProperty MinuteAngleProperty =
            DependencyProperty.Register("MinuteAngle", typeof(double), typeof(ClockSlave1View),
                new FrameworkPropertyMetadata(0d));

        public double SecondAngle
        {
            get { return (double)base.GetValue(SecondAngleProperty); }
            set { base.SetValue(SecondAngleProperty, value); }
        }
        public static readonly DependencyProperty SecondAngleProperty =
            DependencyProperty.Register("SecondAngle", typeof(double), typeof(ClockSlave1View),
                new FrameworkPropertyMetadata(0d));

        public double WeekAngle
        {
            get { return (double)base.GetValue(WeekAngleProperty); }
            set { base.SetValue(WeekAngleProperty, value); }
        }
        public static readonly DependencyProperty WeekAngleProperty =
            DependencyProperty.Register("WeekAngle", typeof(double), typeof(ClockSlave1View),
                new FrameworkPropertyMetadata(0d));

        public ClockSlave1View()
        {
            InitializeComponent();
            this.CreateBoldBorderMark();
            this.CreateNormalBorderMark();
            
            this.DataContext = this;

            this.Loaded += ClockSlave1View_Loaded;
            this.Unloaded += ClockSlave1View_Unloaded;
        }

        private void ClockSlave1View_Unloaded(object sender, RoutedEventArgs e)
        {
            this.FreeClockTimer();
            
            this.CurHour = -1;
            this.CurMinute = -1;
            this.CurSecond = -1;
            this.BeginAnimation(WeekAngleProperty, null);
            this.BeginAnimation(HourAngleProperty, null);
            this.BeginAnimation(MinuteAngleProperty, null);
            this.BeginAnimation(SecondAngleProperty, null);
            this.WeekAngle = 0;
            this.HourAngle = 0;
            this.MinuteAngle = 0;
            this.SecondAngle = 0;
        }

        private void ClockSlave1View_Loaded(object sender, RoutedEventArgs e)
        {
            this.InitWeekAngle();
            this.CreateClockTimer();
        }

        private DispatcherTimer TimerClock;

        private void CreateClockTimer()
        {
            if(this.TimerClock == null)
            {
                this.TimerClock = new DispatcherTimer();
                this.TimerClock.Tick += TimerClock_Tick;
                this.TimerClock.Interval = TimeSpan.FromSeconds(1);
            }
            this.TimerClock.Start();
        }

        private double CurHour = -1;
        private double CurMinute = -1;
        private double CurSecond = -1;

        private void TimerClock_Tick(object sender, EventArgs e)
        {
            DateTime dateNow = DateTime.Now;
            this.TbkTime.Text = DateTime.Now.ToString("s").Replace("T", " ");

            if (this.CurMinute  != dateNow.Minute)
            {
                this.CurMinute = dateNow.Minute;
                double dMinuteAngle = this.CurMinute * 6d;
                if (dMinuteAngle >= 360)
                    dMinuteAngle = 0;

                if (dMinuteAngle == 0)
                {
                    DoubleAnimation animMinute = new DoubleAnimation(-6d, 0, TimeSpan.FromSeconds(0.2));
                    this.BeginAnimation(MinuteAngleProperty, animMinute);
                }
                else
                {
                    DoubleAnimation animMinute = new DoubleAnimation(dMinuteAngle, TimeSpan.FromSeconds(0.2));
                    this.BeginAnimation(MinuteAngleProperty, animMinute);
                }
            }

            if (this.CurSecond != dateNow.Second)
            {
                this.CurSecond = dateNow.Second;
                double dSecondAngle = this.CurSecond * 6d;
                if (dSecondAngle >= 360)
                    dSecondAngle = 0;

                if (dSecondAngle == 0)
                {
                    DoubleAnimation animSecond = new DoubleAnimation(-6, dSecondAngle, TimeSpan.FromSeconds(0.2));
                    this.BeginAnimation(SecondAngleProperty, animSecond);
                }
                else
                {
                    DoubleAnimation animSecond = new DoubleAnimation(dSecondAngle, TimeSpan.FromSeconds(0.2));
                    this.BeginAnimation(SecondAngleProperty, animSecond);
                }
            }

            if (this.CurHour != dateNow.Hour + this.CurMinute/ 60d)
            {
                this.CurHour = dateNow.Hour + this.CurMinute / 60d;
                double dHourAngle = (dateNow.Hour % 12 + this.CurMinute / 60d ) * 30d;
                if (dHourAngle >= 360)
                    dHourAngle = 0;

                if (dHourAngle == 0)
                {
                    DoubleAnimation animHour = new DoubleAnimation(-30, 0, TimeSpan.FromSeconds(0.2));
                    this.BeginAnimation(HourAngleProperty, animHour);
                }
                else
                {
                    DoubleAnimation animHour = new DoubleAnimation(dHourAngle, TimeSpan.FromSeconds(0.2));
                    this.BeginAnimation(HourAngleProperty, animHour);
                }
            }
        }

        private void FreeClockTimer()
        { 
            if(this.TimerClock != null)
            {
                this.TimerClock.Stop();
                this.TimerClock.Tick -= TimerClock_Tick;
            }
            this.TimerClock = null;
        }

        private void CreateBoldBorderMark()
        {
            this.GdEllipse2.Children.Clear();
            double dAngel = 360 / 12d;
            for (int i = 1; i <= 12; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Fill = new SolidColorBrush(Colors.White);
                rect.Width = 2;
                rect.Height = 8;
                rect.RenderTransformOrigin = new Point(0.5, 22.5);

                TransformGroup transGroup = new TransformGroup();
                RotateTransform rotate = new RotateTransform();
                rotate.Angle = i * dAngel;
                TranslateTransform translate = new TranslateTransform();
                translate.X = -0.5;
                translate.Y = -176;
                transGroup.Children.Add(rotate);
                transGroup.Children.Add(translate);
                rect.RenderTransform = transGroup;

                this.GdEllipse2.Children.Add(rect);
            }
        }

        private void CreateNormalBorderMark()
        {
            this.GdEllipse1.Children.Clear();
            
            double dAngel = 6d;
            double nMarkCount = 360 / dAngel;
            for (int i = 1; i <= nMarkCount; i++)
            {
                if (i % 5 != 0)
                {
                    Rectangle rect = new Rectangle();
                    rect.Fill = new SolidColorBrush(Colors.White);
                    rect.Width = 1;
                    rect.Height = 4;
                    rect.RenderTransformOrigin = new Point(0.5, 45);

                    TransformGroup transGroup = new TransformGroup();
                    RotateTransform rotate = new RotateTransform();
                    rotate.Angle = i * dAngel;
                    TranslateTransform translate = new TranslateTransform();
                    translate.X = -0.5;
                    translate.Y = -178;
                    transGroup.Children.Add(rotate);
                    transGroup.Children.Add(translate);
                    rect.RenderTransform = transGroup;

                    this.GdEllipse1.Children.Add(rect);
                }
            }
        }

        private void InitWeekAngle()
        {
            DateTime dateNow = DateTime.Now;
            DayOfWeek dayOfWeek = dateNow.DayOfWeek;
            int nIndex = 0;
            if (dayOfWeek == DayOfWeek.Monday)
                nIndex = 0;
            else if (dayOfWeek == DayOfWeek.Tuesday)
                nIndex = 1;
            else if (dayOfWeek == DayOfWeek.Wednesday)
                nIndex = 2;
            else if (dayOfWeek == DayOfWeek.Thursday)
                nIndex = 3;
            else if (dayOfWeek == DayOfWeek.Friday)
                nIndex = 4;
            else if (dayOfWeek == DayOfWeek.Saturday)
                nIndex = 5;
            else if (dayOfWeek == DayOfWeek.Sunday)
                nIndex = 6;

            double dAngelInterval = 360d / 7d;
            double dWeekAngle = dAngelInterval / 2d + nIndex * dAngelInterval;

            DoubleAnimation animWeek = new DoubleAnimation(dWeekAngle, TimeSpan.FromSeconds(0.2));
            this.BeginAnimation(WeekAngleProperty, animWeek);
        }
    }
}
