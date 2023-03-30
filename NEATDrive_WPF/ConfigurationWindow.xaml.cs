using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NEATDrive_WPF
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        Storyboard fadeInPreview;

        public ConfigurationWindow()
        {
            InitializeComponent();

            fadeInPreview = (Storyboard)FindResource("PreviewPageAnim");
        }

        #region Menu Bar Events
        void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

    }
}
