using NEATDrive_WPF.DrivingScripts;
using NEATDrive_WPF.DrivingScripts.RoadSlots;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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


        private RoadOptionSlot? selectedRoadOptionSlot;

        private RoadOptionSlot? turnRoadOption;
        private RoadOptionSlot? straightRoadOption;
        private RoadOptionSlot? centerRoadOption;
        private RoadOptionSlot? threeWayRoadOption;
        private RoadOptionSlot? nullRoadOption;

        public RoadSlot? RoadSlot1_1;
        public RoadSlot? RoadSlot1_2;
        public RoadSlot? RoadSlot1_3;
        public RoadSlot? RoadSlot2_1;
        public RoadSlot? RoadSlot2_2;
        public RoadSlot? RoadSlot2_3;
        public RoadSlot? RoadSlot3_1;
        public RoadSlot? RoadSlot3_2;
        public RoadSlot? RoadSlot3_3;

        Thickness originalThickness = new Thickness(2);
        Thickness increasedThickness = new Thickness(10);

        //public BitmapImage nullSlotImage = new BitmapImage(new Uri("pack://application:,,,/NEATDrive_WPF;component/Resources/Images/Props/Grass_Cute.png", UriKind.Absolute));

        public List<RoadSlot> roadSlotList = new List<RoadSlot>();


        public ConfigurationWindow()
        {
            InitializeComponent();

            //AddRoadSlotsToList();

            //selectedRoadOptionSlot = new RoadOptionSlot(new Uri("pack://application:,,,/NEATDrive_WPF;component/Resources/Images/Props/Grass_Cute.png", UriKind.Absolute));            //selectedRoadOptionSlot = new RoadOptionSlot(new Uri("pack://application:,,,/NEATDrive_WPF;component/Resources/Images/Props/Grass_Cute.png", UriKind.Absolute));


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
                Start_Border.Visibility = Visibility.Visible;
                InitializeRoadOptions();
            }
            else
            {
                HomeGrid.Visibility = Visibility.Hidden;
                Start_Border.Visibility = Visibility.Collapsed;
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


            //Uri turnRoadUri = new Uri("../../../Resources/Images/Roads/SystemicRoads/Turn_Road.png", UriKind.Relative);
            Uri turnRoadUri = new Uri("pack://application:,,,/NEATDrive_WPF;component/Resources/Images/Roads/SystemicRoads/Turn_Road.png", UriKind.Absolute);
            turnRoadOption = new RoadOptionSlot(turnRoadUri);

            Uri straightRoadUri = new Uri("pack://application:,,,/NEATDrive_WPF;component/Resources/Images/Roads/SystemicRoads/Straight_Road.png", UriKind.Absolute);
            straightRoadOption = new RoadOptionSlot(straightRoadUri);

            Uri centerRoadUri = new Uri("pack://application:,,,/NEATDrive_WPF;component/Resources/Images/Roads/SystemicRoads/Chowk.png", UriKind.Absolute);
            centerRoadOption = new RoadOptionSlot(centerRoadUri);

            Uri threeWayRoadUri = new Uri("pack://application:,,,/NEATDrive_WPF;component/Resources/Images/Roads/SystemicRoads/Tri_Road.png", UriKind.Absolute);
            threeWayRoadOption = new RoadOptionSlot(threeWayRoadUri);

            Uri nullRoadUri = new Uri("pack://application:,,,/NEATDrive_WPF;component/Resources/Images/BackgroundTextures/TiledTexture.jpg", UriKind.Absolute);
            nullRoadOption = new RoadOptionSlot(nullRoadUri);

            Row1_1.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));
            Row1_2.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));
            Row1_3.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));
            Row2_1.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));
            Row2_2.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));
            Row2_3.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));
            Row3_1.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));
            Row3_2.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));
            Row3_3.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));

            RoadSlot1_1 = new RoadSlot(nullRoadOption.BitmapUri, 1, 1);
            RoadSlot1_2 = new RoadSlot(nullRoadOption.BitmapUri, 1, 2);
            RoadSlot1_3 = new RoadSlot(nullRoadOption.BitmapUri, 1, 3);
            RoadSlot2_1 = new RoadSlot(nullRoadOption.BitmapUri, 2, 1);
            RoadSlot2_2 = new RoadSlot(nullRoadOption.BitmapUri, 2, 2);
            RoadSlot2_3 = new RoadSlot(nullRoadOption.BitmapUri, 2, 3);
            RoadSlot3_1 = new RoadSlot(nullRoadOption.BitmapUri, 3, 1);
            RoadSlot3_2 = new RoadSlot(nullRoadOption.BitmapUri, 3, 2);
            RoadSlot3_3 = new RoadSlot(nullRoadOption.BitmapUri, 3, 3);
            selectedRoadOptionSlot = nullRoadOption;
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
            //RoadManager.instance?.roadConfiguration.
            AddRoadSlotsToList();
            RoadManager.instance?.RefreshRoadConfig();

            this.Hide();
            ApplicationManager.instance?.simWindow.ShowDialog();
            //ApplicationManager.instance?.simWindow.Focus();

            //ApplicationManager.instance?.configWindow.Hide();
            //EnableSimGrid(true);
            //ApplicationManager.instance?.FocusCanvas(SimulationCanvas);
            //driveManager = new(this);
            /*if (driveManager.isSimStart())
            {*/
            //driveManager.InitTimer();

            //driveManager.StartSim();

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
                RoadSlot1_1 = new RoadSlot(selectedRoadOptionSlot.BitmapUri, 1, 1);
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
                    RoadSlot1_1 = new RoadSlot(nullRoadOption.BitmapUri, 1, 1);
                    Row1_1.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));
                }
            }
            //RoadManager.instance?.RefreshRoadConfig();
        }

        private void Row1_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RoadSlot1_2 = new RoadSlot(selectedRoadOptionSlot.BitmapUri, 1, 2);
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
                    RoadSlot1_2 = new RoadSlot(nullRoadOption.BitmapUri, 1, 2);
                    Row1_2.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));
                }
            }
        }

        private void Row1_3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RoadSlot1_3 = new RoadSlot(selectedRoadOptionSlot.BitmapUri, 1, 3);
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
                    RoadSlot1_3 = new RoadSlot(nullRoadOption.BitmapUri, 1, 3);
                    Row1_3.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));
                }
            }
        }

        private void Row2_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RoadSlot2_1 = new RoadSlot(selectedRoadOptionSlot.BitmapUri, 2, 1);
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
                    RoadSlot2_1 = new RoadSlot(nullRoadOption.BitmapUri, 2, 1);
                    Row2_1.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));
                }
            }
        }

        private void Row2_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RoadSlot2_2 = new RoadSlot(selectedRoadOptionSlot.BitmapUri, 2, 2);
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
                    RoadSlot2_2 = new RoadSlot(nullRoadOption.BitmapUri, 2, 2);
                    Row2_2.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));
                }
            }
        }

        private void Row2_3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RoadSlot2_3 = new RoadSlot(selectedRoadOptionSlot.BitmapUri, 2, 3);
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
                    RoadSlot2_3 = new RoadSlot(nullRoadOption.BitmapUri, 2, 3);
                    Row2_3.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));
                }
            }
        }

        private void Row3_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RoadSlot3_1 = new RoadSlot(selectedRoadOptionSlot.BitmapUri, 3, 1);
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
                    RoadSlot3_1 = new RoadSlot(nullRoadOption.BitmapUri, 3, 1);
                    Row3_1.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));
                }
            }
        }

        private void Row3_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RoadSlot3_2 = new RoadSlot(selectedRoadOptionSlot.BitmapUri, 3, 2);
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
                    RoadSlot3_2 = new RoadSlot(nullRoadOption.BitmapUri, 3, 2);
                    Row3_2.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));
                }
            }
        }

        private void Row3_3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RoadSlot3_3 = new RoadSlot(selectedRoadOptionSlot.BitmapUri, 3, 3);
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
                    RoadSlot3_3 = new RoadSlot(nullRoadOption.BitmapUri, 3, 3);
                    Row3_3.Background = new ImageBrush(new BitmapImage(nullRoadOption.BitmapUri));
                }
            }
        }

        #endregion

        #region Slot Selection

        private void AddRoadSlotIfNotExists(List<RoadSlot> roadSlotList, RoadSlot roadSlot)
        {
            if (!roadSlotList.Equals(roadSlot))
            {
                roadSlotList.Add(roadSlot);
            }
        }

        private void UpdateRoadSlotInList(List<RoadSlot> roadSlotList, RoadSlot roadSlotToUpdate)
        {
            int index = roadSlotList.IndexOf(roadSlotToUpdate);
            if (index >= 0)
            {
                // Modify the existing RoadSlot instance within the list
                roadSlotList[index] = roadSlotToUpdate;
            }
            else
            {
                // Handle the case when the RoadSlot instance is not found in the list
                // Example: HandleNotFound();
            }
        }
        public void UpdateRoadSlotsList()
        {
            UpdateRoadSlotInList(roadSlotList, RoadSlot1_1);
            UpdateRoadSlotInList(roadSlotList, RoadSlot1_2);
            UpdateRoadSlotInList(roadSlotList, RoadSlot1_3);
            UpdateRoadSlotInList(roadSlotList, RoadSlot2_1);
            UpdateRoadSlotInList(roadSlotList, RoadSlot2_2);
            UpdateRoadSlotInList(roadSlotList, RoadSlot2_3);
            UpdateRoadSlotInList(roadSlotList, RoadSlot3_1);
            UpdateRoadSlotInList(roadSlotList, RoadSlot3_2);
            UpdateRoadSlotInList(roadSlotList, RoadSlot3_3);
        }

        public void AddRoadSlotsToList()
        {
            AddRoadSlotIfNotExists(roadSlotList, RoadSlot1_1);
            AddRoadSlotIfNotExists(roadSlotList, RoadSlot1_2);
            AddRoadSlotIfNotExists(roadSlotList, RoadSlot1_3);
            AddRoadSlotIfNotExists(roadSlotList, RoadSlot2_1);
            AddRoadSlotIfNotExists(roadSlotList, RoadSlot2_2);
            AddRoadSlotIfNotExists(roadSlotList, RoadSlot2_3);
            AddRoadSlotIfNotExists(roadSlotList, RoadSlot3_1);
            AddRoadSlotIfNotExists(roadSlotList, RoadSlot3_2);
            AddRoadSlotIfNotExists(roadSlotList, RoadSlot3_3);
            Debug.WriteLine(roadSlotList.Count + "This is Sparta!");

            //for (int i = 1; i <= 3; i++)
            //{
            //    for (int j = 1; j <= 3; j++)
            //    {
            //        RoadSlot? roadSlot = FindName("RoadSlot" + i + "_" + j) as RoadSlot;
            //        Debug.WriteLine(roadSlot);
            //        if (!roadSlotList.Contains(roadSlot))
            //        {
            //            roadSlotList.Add(roadSlot);
            //        }
            //    }
            //}
        }





        #endregion

        private void Export_Metrics_Btn_Click(object sender, RoutedEventArgs e)
        {
            //SaveManager.instance?.SaveToCSVSheet();
            //Debug.WriteLine("Saved to csv");

            if (SpreadSheet_Option.SelectedItem == SpreadSheet_Option.Items[0])
            {
                SaveManager.instance?.SaveToExcelSheet();
                Debug.WriteLine("Saved to excel");
            }
            else if (SpreadSheet_Option.SelectedItem == SpreadSheet_Option.Items[1])
            {
                SaveManager.instance?.SaveToCSVSheet();
                Debug.WriteLine("Saved to csv");
            }
        }
    }
}