using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// AboutMeModuleView.xaml 的交互逻辑
    /// </summary>
    public partial class AboutMeModuleView : BaseModuleView
    {
        public AboutMeModuleView()
        {
            InitializeComponent();

            this.MenuToMall.MouseLeftButtonDown += MenuToMall_MouseLeftButtonDown;

            this.Loaded += AboutMeModuleView_Loaded;
            this.Unloaded += AboutMeModuleView_Unloaded;
        }

        private void AboutMeModuleView_Unloaded(object sender, RoutedEventArgs e)
        {
            StopFrameAnim();
        }

        private void AboutMeModuleView_Loaded(object sender, RoutedEventArgs e)
        {
            PlayFrameAnim();
        }

        private void MenuToMall_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "https://shop173071246.taobao.com/index.htm";
            proc.Start();
        }

        private void PlayFrameAnim()
        {
            Storyboard anim = this.TryFindResource("Sb.BgAnim") as Storyboard;
            if (anim != null)
                anim.Begin();
        }

        private void StopFrameAnim()
        {
            Storyboard anim = this.TryFindResource("Sb.BgAnim") as Storyboard;
            if (anim != null)
                anim.Stop();
        }
    }
}
