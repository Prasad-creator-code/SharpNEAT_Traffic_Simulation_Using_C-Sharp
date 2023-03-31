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

            DisableOnStart();
            fadeInPreview = (Storyboard)FindResource("PreviewPageAnim");
            fadeInPreview.Begin();
        }

        #region Activation Methods
        void EnableSimGrid(bool enable)
        {
            if (enable)
            {
                SimGrid.Visibility = Visibility.Visible;
            }
            else
            {
                SimGrid.Visibility = Visibility.Hidden;
            }

        }
        #endregion

        #region Enable These
        void EnableOnStart()
        {

        }
        #endregion

        #region Disable These
        void DisableOnStart()
        {
            EnableSimGrid(false);
        }

        #endregion

        #region On Config Window Events
        private void ConfigurationWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            /// <summary>
            /// On Configuration Window is Loaded
            /// </summary>
            
            fadeInPreview.Begin();
            DisableOnStart();
        }
        #endregion

        #region Menu Bar Events
        void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        #endregion

        #region Left Side Options Bar Events
        private void HomeButton_Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Start_Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EnableSimGrid(true);
        }
        #endregion


    }
}
