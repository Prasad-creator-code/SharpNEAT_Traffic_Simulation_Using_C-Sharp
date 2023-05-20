using NEATDrive_WPF.DrivingScripts;
using NEATDrive_WPF.DrivingScripts.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NEATDrive_WPF
{
    /// <summary>
    /// Interaction logic for SimulationWindow.xaml
    /// </summary>
    public partial class SimulationWindow : Window
    {
        public SimulationWindow()
        {
            InitializeComponent();
            AddRoadBordersToList();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            Stop_Sim_Button_Border.Visibility = Visibility.Collapsed;

        }
        public List<Canvas> roadCanvasList = new List<Canvas>();

        public void AddRoadBordersToList()
        {
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    string canvasName = "Road" + i.ToString() + "_" + j.ToString();
                    Canvas canvas = (Canvas)FindName(canvasName);

                    if (canvas != null && !roadCanvasList.Contains(canvas))
                    {
                        roadCanvasList.Add(canvas);
                    }
                }
            }
        }

        internal void SimLoop(object? sender, EventArgs e)
        {
            //DriveManager.instance?.UpdateCarPosition();

        }

        private void OnWindowClose(object sender, EventArgs e)
        {
            //ApplicationManager.instance?.simWindow.Hide();
            this.Hide();
            ApplicationManager.instance?.configWindow.Show();
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
            //ApplicationManager.instance?.configWindow.Focus();
        }



        private void Start_Sim_Button_Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DriveManager.instance?.StartSim();
            Start_Sim_Button_Border.Visibility = Visibility.Collapsed;
            Stop_Sim_Button_Border.Visibility = Visibility.Visible;
            HeroCar_Sprite.Focus();
        }

        private void Start_Sim_Button_Border_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Start_Sim_Text.FontSize = MathUtils.Lerp(18, 20, 1);

        }

        private void Start_Sim_Button_Border_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Start_Sim_Text.FontSize = MathUtils.Lerp(20, 18, 1);

        }




        private void Stop_Sim_Button_Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DriveManager.instance?.StopSim();
            Start_Sim_Button_Border.Visibility = Visibility.Visible;
            Stop_Sim_Button_Border.Visibility = Visibility.Collapsed;
        }

        private void Stop_Sim_Button_Border_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Stop_Sim_Text.FontSize = MathUtils.Lerp(18, 20, 1);

        }

        private void Stop_Sim_Button_Border_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Stop_Sim_Text.FontSize = MathUtils.Lerp(20, 18, 1);

        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            // Update the car's position and perform other game logic
            DriveManager.instance?.heroCar.Update();
        }

        private void HeroCar_Sprite_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.W)
            {
                // Accelerate forward
                DriveManager.instance?.heroCar.Accelerate();

            }
            else if (e.Key == Key.Down || e.Key == Key.S)
            {
                // Accelerate backward
                DriveManager.instance?.heroCar.Decelerate();
            }
            else if (e.Key == Key.Left || e.Key == Key.A)
            {
                // Turn left
                DriveManager.instance?.heroCar.TurnLeft();
            }
            else if (e.Key == Key.Right || e.Key == Key.D)
            {
                // Turn right
                DriveManager.instance?.heroCar.TurnRight();
            }
        }

        private void HeroCar_Sprite_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.W || e.Key == Key.Down || e.Key == Key.S)
            {
                // Release acceleration
                //DriveManager.instance?.heroCar.ReleaseAcceleration();
            }
            else if (e.Key == Key.Left || e.Key == Key.A || e.Key == Key.Right || e.Key == Key.D)
            {
                // Release turning
                //DriveManager.instance?.heroCar.ReleaseTurning();
            }
        }

        public static bool IsAccelerationKeyDown()
        {
            return Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.S);
        }

    }
}
