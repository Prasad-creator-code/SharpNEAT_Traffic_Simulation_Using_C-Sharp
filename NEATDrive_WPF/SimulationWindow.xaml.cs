using NEATDrive_WPF.DrivingScripts;
using NEATDrive_WPF.DrivingScripts.CarScripts.HeroCar;
using NEATDrive_WPF.DrivingScripts.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

            //CompositionTarget.Rendering += CompositionTarget_Rendering;
            Stop_Sim_Button_Border.Visibility = Visibility.Collapsed;


        }
        public List<Canvas> roadCanvasList = new List<Canvas>();
        private double carSpeed;
        private double carRotation;
        private double carTurningSpeed = 2.5;
        private double carFriction;
        private double carPositionX;
        private double carPositionY;
        private double carMaxSpeed = 5;
        protected const double Acceleration = 0.25;
        protected const double Deceleration = 0.05;
        //protected const double MaxSpeed = 6;
        protected const double TurningAngle = 15;
        //protected const double Handling = 0.15;

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

            CompositionTarget.Rendering += CompositionTarget_Rendering;
            AssignMergedRoadImage();
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
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
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
            //DriveManager.instance?.heroCar.Update();
            UpdateCarPosition();
        }
        public void UpdateCarPosition()
        {
            double carX = Canvas.GetLeft(HeroCar_Sprite);
            double carY = Canvas.GetTop(HeroCar_Sprite);

            Color underlyingColor = GetUnderlyingColor(HeroCar_Sprite);

            //// Check if the underlying color is green (grass)
            if (IsColorGreen(underlyingColor))
            {
                carMaxSpeed = 0.1;
                Debug.WriteLine("Yes Color Found ");
            }
            else
            {

                carMaxSpeed = 5;
                Debug.WriteLine("NO, it is " + "R " + underlyingColor.R + "G " + underlyingColor.G + "B " + underlyingColor.G);
            }


            if (IsTurnLeftKeyDown() && (carSpeed > 0 || carSpeed < 0))
            {
                carRotation -= carTurningSpeed;
            }
            if (IsTurnRightKeyDown() && (carSpeed > 0 || carSpeed < 0))
            {
                carRotation += carTurningSpeed;
            }
            if (IsAccelerationKeyDown())
            {

                carSpeed += Acceleration;
                if (carSpeed > carMaxSpeed)
                {
                    carSpeed = carMaxSpeed;
                }
            }

            if (IsBrakeKeyDown())
            {
                carSpeed -= Deceleration;
                if (carSpeed <= 0)
                {
                    carSpeed = -carMaxSpeed / 2;
                }
            }



            double carDirectionX = Math.Cos(carRotation * Math.PI / 180);
            double carDirectionY = Math.Sin(carRotation * Math.PI / 180);

            carPositionX += carDirectionX * carSpeed;
            carPositionY += carDirectionY * carSpeed;

            Canvas.SetLeft(HeroCar_Sprite, carPositionX);
            Canvas.SetTop(HeroCar_Sprite, carPositionY);

            double angle = Math.Atan2(carDirectionY, carDirectionX) + Math.PI / 2;
            angle *= 180 / Math.PI;
            angle = Math.Round(angle, 2);
            RotateTransform rotation = new RotateTransform(angle);

            HeroCar_Sprite.RenderTransform = rotation;





            if (IsAccelerationKeyUp() && IsBrakeKeyUp())
            {

                if (Math.Abs(carSpeed) > 0)
                {
                    carSpeed -= Math.Sign(carSpeed) * 0.1;

                    if (Math.Abs(carSpeed) < 0.1)
                    {
                        carSpeed = 0;
                    }
                }
                if (Math.Abs(carSpeed) < 0)
                {
                    carSpeed += Math.Sign(carSpeed) * 0.1;

                    if (Math.Abs(carSpeed) > 0.1)
                    {
                        carSpeed = 0;
                    }
                }

                if (IsAccelerationKeyUp() && carSpeed > 0)
                {
                    carSpeed -= Deceleration;
                }
                if (IsBrakeKeyUp() && carSpeed < 0)
                {

                    carSpeed += Deceleration;
                }


            }
        }

        // Get the color of the underlying image at the specified position in the given canvas
        Color GetUnderlyingColor(Canvas carCanvas)
        {
            // Get the position of the carCanvas within its parent canvas
            Point carCanvasPosition = carCanvas.TransformToVisual(RoadCanvas).Transform(new Point(0, 0));

            // Convert the carCanvas position to integer coordinates
            int carCanvasX = (int)carCanvasPosition.X;
            int carCanvasY = (int)carCanvasPosition.Y;

            // Check if the carCanvas is within the bounds of RoadCanvas
            if (carCanvasX >= 0 && carCanvasX < RoadCanvas.ActualWidth && carCanvasY >= 0 && carCanvasY < RoadCanvas.ActualHeight)
            {
                // Create a RenderTargetBitmap of RoadCanvas
                RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)RoadCanvas.ActualWidth, (int)RoadCanvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                renderBitmap.Render(RoadCanvas);

                // Get the color of the pixel at the carCanvas position
                CroppedBitmap croppedBitmap = new CroppedBitmap(renderBitmap, new Int32Rect(carCanvasX, carCanvasY, 1, 1));
                byte[] pixelColor = new byte[4];
                croppedBitmap.CopyPixels(pixelColor, 4, 0);

                // Check if the color is green
                if (pixelColor[2] == 0 && pixelColor[1] == 255 && pixelColor[0] == 0)
                {
                    return Colors.Green; // Return green color
                }
            }

            // Return a default color (e.g., transparent) if no green color is detected or the carCanvas is outside the bounds of RoadCanvas
            return Colors.Transparent;
        }




        // Check if the color is green (grass)
        bool IsColorGreen(Color color)
        {
            return color.R == 60 && color.G == 150 && color.B == 60; // Adjust the color values as needed
        }




        //Debug.WriteLine(carSpeed);


        RenderTargetBitmap CombineRoadCanvases()
        {
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)RoadCanvas.ActualWidth, (int)RoadCanvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            DrawingVisual drawingVisual = new DrawingVisual();

            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                foreach (Canvas roadCanvas in RoadCanvas.Children)
                {
                    // Get the position of the roadCanvas within the RoadCanvas
                    Point roadCanvasPosition = roadCanvas.TransformToAncestor(RoadCanvas).Transform(new Point(0, 0));

                    // Get the background image source of the roadCanvas
                    ImageSource backgroundImage = ((ImageBrush)roadCanvas.Background).ImageSource;

                    // Draw the background image onto the RenderTargetBitmap
                    drawingContext.DrawImage(backgroundImage, new Rect(roadCanvasPosition, roadCanvas.RenderSize));
                }
                drawingContext.Close();
            }


            renderBitmap.Render(drawingVisual);

            return renderBitmap;
        }


        private void AssignMergedRoadImage()
        {

            RenderTargetBitmap mergedBitmap = CombineRoadCanvases();

            Image roadImage = new Image();
            roadImage.Source = mergedBitmap;

            RoadCanvas.Children.Clear();
            RoadCanvas.Children.Add(roadImage);
        }

        private void HeroCar_Sprite_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.W)
            {
                // Accelerate forward
                //DriveManager.instance?.heroCar.Accelerate();
                //Accelerate();

            }
            else if (e.Key == Key.Down || e.Key == Key.S)
            {
                // Accelerate backward
                //DriveManager.instance?.heroCar.Decelerate();
                //Decelerate();
            }
            else if (e.Key == Key.Left || e.Key == Key.A)
            {
                // Turn left
                //DriveManager.instance?.heroCar.TurnLeft();
                //TurnRight();
            }
            else if (e.Key == Key.Right || e.Key == Key.D)
            {
                // Turn right
                //DriveManager.instance?.heroCar.TurnRight();
                //TurnRight();
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

        public void Accelerate()
        {
            //speed += Acceleration;

        }

        public void Decelerate()
        {
            //speed -= Deceleration;
        }

        public void TurnLeft()
        {
            //angle -= TurningAngle;
        }

        public void TurnRight()
        {
            //angle += TurningAngle;
        }

        bool IsAccelerationKeyDown()
        {
            return Keyboard.IsKeyDown(Key.Up);
        }
        bool IsAccelerationKeyUp()
        {
            return Keyboard.IsKeyUp(Key.Up);
        }
        bool IsBrakeKeyDown()
        {

            return Keyboard.IsKeyDown(Key.Down);
        }
        bool IsBrakeKeyUp()
        {

            return Keyboard.IsKeyUp(Key.Down);
        }

        bool IsTurnRightKeyDown()
        {
            return Keyboard.IsKeyDown(Key.Right);
        }

        bool IsTurnLeftKeyDown()
        {
            return Keyboard.IsKeyDown(Key.Left);
        }



        private void SimulationWindow1_Activated(object sender, EventArgs e)
        {
            DriveManager.instance.heroCar = new HeroCar(HeroCar_Sprite);

            //DriveManager.instance.heroCar = new HeroCar(HeroCar_Sprite);

        }

        private void SimulationWindow1_Deactivated(object sender, EventArgs e)
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }
    }
}
