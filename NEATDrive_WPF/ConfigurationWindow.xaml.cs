using NEATDrive_WPF.DrivingScripts;
using NEATDrive_WPF.DrivingScripts.RoadSlots;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace NEATDrive_WPF
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        readonly Storyboard fadeInPreview;

        DriveManager? driveManager;

        private RoadOptionSlot? selectedRoadOptionSlot;

        private RoadOptionSlot? turnRoadOption;
        private RoadOptionSlot? straightRoadOption;
        private RoadOptionSlot? centerRoadOption;
        private RoadOptionSlot? threeWayRoadOption;

        private RoadSlot? RoadSlot1_1;
        private RoadSlot? RoadSlot1_2;
        private RoadSlot? RoadSlot1_3;
        private RoadSlot? RoadSlot2_1;
        private RoadSlot? RoadSlot2_2;
        private RoadSlot? RoadSlot2_3;
        private RoadSlot? RoadSlot3_1;
        private RoadSlot? RoadSlot3_2;
        private RoadSlot? RoadSlot3_3;

        Thickness originalThickness = new Thickness(2);
        Thickness increasedThickness = new Thickness(10);

        private BitmapImage nullSlotImage = new BitmapImage(new Uri("pack://application:,,,/NEATDrive_WPF;component/Resources/Images/Props/Grass_Cute.png", UriKind.Absolute));



        public ConfigurationWindow()
        {
            InitializeComponent();

            DisableOnStart();
            fadeInPreview = (Storyboard)FindResource("PreviewPageAnim");
            fadeInPreview.Begin();
        }

        #region Activation Methods
        void EnableSimGrid(bool state)
        {
            if (state)
            {
                HomeGrid.Visibility = Visibility.Visible;
                InitializeRoadOptions();
            }
            else
            {
                HomeGrid.Visibility = Visibility.Hidden;
            }

        }
        void EnableParamGrid(bool state)
        {
            if (state)
            {
                ParametersGrid.Visibility = Visibility.Visible;
            }
            else
            {
                ParametersGrid.Visibility = Visibility.Hidden;
            }
        }
        void EnableMetricsGrid(bool state)
        {
            if (state)
            {
                MetricsGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MetricsGrid.Visibility = Visibility.Hidden;
            }
        }

        void InitializeRoadOptions()
        {
            Row1_1.Background = new ImageBrush(nullSlotImage);
            Row1_2.Background = new ImageBrush(nullSlotImage);
            Row1_3.Background = new ImageBrush(nullSlotImage);
            Row2_1.Background = new ImageBrush(nullSlotImage);
            Row2_2.Background = new ImageBrush(nullSlotImage);
            Row2_3.Background = new ImageBrush(nullSlotImage);
            Row3_1.Background = new ImageBrush(nullSlotImage);
            Row3_2.Background = new ImageBrush(nullSlotImage);
            Row3_3.Background = new ImageBrush(nullSlotImage);

            //Uri turnRoadUri = new Uri("../../../Resources/Images/Roads/SystemicRoads/Turn_Road.png", UriKind.Relative);
            Uri turnRoadUri = new Uri("pack://application:,,,/NEATDrive_WPF;component/Resources/Images/Roads/SystemicRoads/Turn_Road.png", UriKind.Absolute);
            turnRoadOption = new RoadOptionSlot(turnRoadUri);

            Uri straightRoadUri = new Uri("pack://application:,,,/NEATDrive_WPF;component/Resources/Images/Roads/SystemicRoads/Straight_Road.png", UriKind.Absolute);
            straightRoadOption = new RoadOptionSlot(straightRoadUri);

            Uri centerRoadUri = new Uri("pack://application:,,,/NEATDrive_WPF;component/Resources/Images/Roads/SystemicRoads/Chowk.png", UriKind.Absolute);
            centerRoadOption = new RoadOptionSlot(centerRoadUri);

            Uri threeWayRoadUri = new Uri("pack://application:,,,/NEATDrive_WPF;component/Resources/Images/Roads/SystemicRoads/Tri_Road.png", UriKind.Absolute);
            threeWayRoadOption = new RoadOptionSlot(threeWayRoadUri);
        }

        #endregion

        #region Enable These
        static void EnableOnStart()
        {

        }
        #endregion

        #region Disable These
        void DisableOnStart()
        {
            EnableSimGrid(false);
            EnableMetricsGrid(false);
            EnableParamGrid(false);
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
            EnableSimGrid(true);
            EnableParamGrid(false);
            EnableMetricsGrid(false);
        }

        private void ParametersButton_Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EnableParamGrid(true);
            EnableSimGrid(false);
            EnableMetricsGrid(false);
        }

        private void MetricsButton_Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EnableMetricsGrid(true);
            EnableParamGrid(false);
            EnableSimGrid(false);

        }
        private void AboutButton_Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        #endregion

        #region Right Side Options Bar Events

        private void Start_Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ApplicationManager.instance?.simWindow.Show();
            ApplicationManager.instance?.configWindow.Hide();
            //EnableSimGrid(true);
            //ApplicationManager.instance?.FocusCanvas(SimulationCanvas);
            //driveManager = new(this);
            /*if (driveManager.isSimStart())
            {*/
            //driveManager.InitTimer();

            //driveManager.StartSim();

        }

        internal void SimLoop(object? sender, EventArgs e)
        {
            //driveManager.UpdateCarPosition();
        }

        #endregion

        #region Window Sim Controls
        private void ConfigurationWindow1_KeyDown(object sender, KeyEventArgs e)
        {
            //driveManager.DirectionalDrivePress(e);

        }
        private void ConfigurationWindow1_KeyUp(object sender, KeyEventArgs e)
        {
            //driveManager.DirectionalDriveRelease(e);

        }

        #endregion


        #region Road Options Events

        #region Turn Road
        private void TurnRoad_Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectedRoadOptionSlot = turnRoadOption;
            TurnRoad_Border.BorderThickness = increasedThickness;

            StraightRoad_Border.BorderThickness = originalThickness;
            CenterRoad_Border.BorderThickness = originalThickness;
            ThreeWayRoad_Border.BorderThickness = originalThickness;
        }

        private void TurnRoad_Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //selectedRoadOption = null;
        }
        #endregion

        #region Straight Road

        private void StraightRoad_Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectedRoadOptionSlot = straightRoadOption;
            StraightRoad_Border.BorderThickness = increasedThickness;

            TurnRoad_Border.BorderThickness = originalThickness;
            CenterRoad_Border.BorderThickness = originalThickness;
            ThreeWayRoad_Border.BorderThickness = originalThickness;

        }

        private void StraightRoad_Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //selectedRoadOption = null;
        }
        #endregion

        #region Center Road

        private void CenterRoad_Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectedRoadOptionSlot = centerRoadOption;
            CenterRoad_Border.BorderThickness = increasedThickness;

            TurnRoad_Border.BorderThickness = originalThickness;
            StraightRoad_Border.BorderThickness = originalThickness;
            ThreeWayRoad_Border.BorderThickness = originalThickness;
        }

        private void CenterRoad_Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //selectedRoadOption = null;
        }
        #endregion

        #region ThreeWay Road

        private void ThreeWayRoad_Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectedRoadOptionSlot = threeWayRoadOption;
            ThreeWayRoad_Border.BorderThickness = increasedThickness;

            TurnRoad_Border.BorderThickness = originalThickness;
            StraightRoad_Border.BorderThickness = originalThickness;
            CenterRoad_Border.BorderThickness = originalThickness;
        }

        private void ThreeWayRoad_Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //selectedRoadOption = null;
        }


        #endregion

        #endregion

        #region Road Row Events

        private void Row1_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RoadSlot1_1 = new RoadSlot(selectedRoadOptionSlot.BitmapUri);
                Row1_1.Background = new ImageBrush(RoadSlot1_1.SelectedImage);
            }
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                if (Row1_1.Background != null)
                {
                    if (RoadSlot1_1.currentRotationAngle >= 360)
                    {
                        RoadSlot1_1.currentRotationAngle = 0;
                    }
                    RoadSlot1_1.currentRotationAngle += 90;
                    Row1_1.Background.RelativeTransform = new RotateTransform(RoadSlot1_1.currentRotationAngle, 0.5, 0.5);
                }
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (Row1_1.Background != null)
                {
                    Row1_1.Background = new ImageBrush(nullSlotImage);
                }
            }
        }

        private void Row1_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RoadSlot1_2 = new RoadSlot(selectedRoadOptionSlot.BitmapUri);
                Row1_2.Background = new ImageBrush(RoadSlot1_2.SelectedImage);
            }
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                if (Row1_2.Background != null)
                {
                    if (RoadSlot1_2.currentRotationAngle >= 360)
                    {
                        RoadSlot1_2.currentRotationAngle = 0;
                    }
                    RoadSlot1_2.currentRotationAngle += 90;
                    Row1_2.Background.RelativeTransform = new RotateTransform(RoadSlot1_2.currentRotationAngle, 0.5, 0.5);
                }
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (Row1_2.Background != null)
                {
                    Row1_2.Background = new ImageBrush(nullSlotImage);
                }
            }
        }

        private void Row1_3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RoadSlot1_3 = new RoadSlot(selectedRoadOptionSlot.BitmapUri);
                Row1_3.Background = new ImageBrush(RoadSlot1_3.SelectedImage);
            }
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                if (Row1_3.Background != null)
                {
                    if (RoadSlot1_3.currentRotationAngle >= 360)
                    {
                        RoadSlot1_3.currentRotationAngle = 0;
                    }
                    RoadSlot1_3.currentRotationAngle += 90;
                    Row1_3.Background.RelativeTransform = new RotateTransform(RoadSlot1_3.currentRotationAngle, 0.5, 0.5);
                }
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (Row1_3.Background != null)
                {
                    Row1_3.Background = new ImageBrush(nullSlotImage);
                }
            }
        }

        private void Row2_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RoadSlot2_1 = new RoadSlot(selectedRoadOptionSlot.BitmapUri);
                Row2_1.Background = new ImageBrush(RoadSlot2_1.SelectedImage);
            }
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                if (Row2_1.Background != null)
                {
                    if (RoadSlot2_1.currentRotationAngle >= 360)
                    {
                        RoadSlot2_1.currentRotationAngle = 0;
                    }
                    RoadSlot2_1.currentRotationAngle += 90;
                    Row2_1.Background.RelativeTransform = new RotateTransform(RoadSlot2_1.currentRotationAngle, 0.5, 0.5);
                }
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (Row2_1.Background != null)
                {
                    Row2_1.Background = new ImageBrush(nullSlotImage);
                }
            }
        }

        private void Row2_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RoadSlot2_2 = new RoadSlot(selectedRoadOptionSlot.BitmapUri);
                Row2_2.Background = new ImageBrush(RoadSlot2_2.SelectedImage);
            }
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                if (Row2_2.Background != null)
                {
                    if (RoadSlot2_2.currentRotationAngle >= 360)
                    {
                        RoadSlot2_2.currentRotationAngle = 0;
                    }
                    RoadSlot2_2.currentRotationAngle += 90;
                    Row2_2.Background.RelativeTransform = new RotateTransform(RoadSlot2_2.currentRotationAngle, 0.5, 0.5);
                }
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (Row2_2.Background != null)
                {
                    Row2_2.Background = new ImageBrush(nullSlotImage);
                }
            }
        }

        private void Row2_3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RoadSlot2_3 = new RoadSlot(selectedRoadOptionSlot.BitmapUri);
                Row2_3.Background = new ImageBrush(RoadSlot2_3.SelectedImage);
            }
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                if (Row2_3.Background != null)
                {
                    if (RoadSlot2_3.currentRotationAngle >= 360)
                    {
                        RoadSlot2_3.currentRotationAngle = 0;
                    }
                    RoadSlot2_3.currentRotationAngle += 90;
                    Row2_3.Background.RelativeTransform = new RotateTransform(RoadSlot2_3.currentRotationAngle, 0.5, 0.5);
                }
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (Row2_3.Background != null)
                {
                    Row2_3.Background = new ImageBrush(nullSlotImage);
                }
            }
        }

        private void Row3_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RoadSlot3_1 = new RoadSlot(selectedRoadOptionSlot.BitmapUri);
                Row3_1.Background = new ImageBrush(RoadSlot3_1.SelectedImage);
            }
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                if (Row3_1.Background != null)
                {
                    if (RoadSlot3_1.currentRotationAngle >= 360)
                    {
                        RoadSlot3_1.currentRotationAngle = 0;
                    }
                    RoadSlot3_1.currentRotationAngle += 90;
                    Row3_1.Background.RelativeTransform = new RotateTransform(RoadSlot3_1.currentRotationAngle, 0.5, 0.5);
                }
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (Row3_1.Background != null)
                {
                    Row3_1.Background = new ImageBrush(nullSlotImage);
                }
            }
        }

        private void Row3_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RoadSlot3_2 = new RoadSlot(selectedRoadOptionSlot.BitmapUri);
                Row3_2.Background = new ImageBrush(RoadSlot3_2.SelectedImage);
            }
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                if (Row3_2.Background != null)
                {
                    if (RoadSlot3_2.currentRotationAngle >= 360)
                    {
                        RoadSlot3_2.currentRotationAngle = 0;
                    }
                    RoadSlot3_2.currentRotationAngle += 90;
                    Row3_2.Background.RelativeTransform = new RotateTransform(RoadSlot3_2.currentRotationAngle, 0.5, 0.5);
                }
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (Row3_2.Background != null)
                {
                    Row3_2.Background = new ImageBrush(nullSlotImage);
                }
            }
        }

        private void Row3_3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RoadSlot3_3 = new RoadSlot(selectedRoadOptionSlot.BitmapUri);
                Row3_3.Background = new ImageBrush(RoadSlot3_3.SelectedImage);
            }
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                if (Row3_3.Background != null)
                {
                    if (RoadSlot3_3.currentRotationAngle >= 360)
                    {
                        RoadSlot3_3.currentRotationAngle = 0;
                    }
                    RoadSlot3_3.currentRotationAngle += 90;
                    Row3_3.Background.RelativeTransform = new RotateTransform(RoadSlot3_3.currentRotationAngle, 0.5, 0.5);
                }
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (Row3_3.Background != null)
                {
                    Row3_2.Background = new ImageBrush(nullSlotImage);
                }
            }
        }

        #endregion




    }
}