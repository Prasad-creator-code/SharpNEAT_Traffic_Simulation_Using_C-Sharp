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

namespace NEATDrive_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Storyboard fadeInCanvas;
        Storyboard loadRotation;

        public MainWindow()
        {
            InitializeComponent();

            fadeInCanvas = (Storyboard)FindResource("FadeIn");
            loadRotation = (Storyboard)FindResource("LoadingRotation");

            loadRotation.Completed += (sender, args) =>
            {
                LoadConfigForm();
            };
        }

        
        private void Canva_Loaded(object sender, RoutedEventArgs e)
        {
            fadeInCanvas.Begin();
            loadRotation.Begin();
        }

        private void LoadConfigForm()
        {
            this.Hide();
            ApplicationManager.instance?.configWindow.Show();
        }

    }
}
