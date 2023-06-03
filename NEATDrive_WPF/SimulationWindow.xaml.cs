using NEATDrive_WPF.DrivingScripts;
using NEATDrive_WPF.DrivingScripts.CarScripts.CivilianCar;
using NEATDrive_WPF.DrivingScripts.CarScripts.HeroCar;
using NEATDrive_WPF.DrivingScripts.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;

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
        NewRaycast newRaycast;

        double minScale = 0.1;
        double maxScale = 100;

        public List<Canvas> roadCanvasList = new();
        private double carSpeed;
        private double carRotation;
        private double carTurningSpeed = 2.5;
        private double carFriction;
        private double carPositionX;
        private double carPositionY;
        private double carMaxSpeed = 5;
        protected const double Acceleration = 0.25;
        protected const double Deceleration = 0.1;
        //protected const double MaxSpeed = 6;
        protected const double TurningAngle = 15;
        //protected const double Handling = 0.15;

        Color underlyingColor = Color.Transparent; // Default color
        bool isColorCacheValid = false;

        private double colorCheckInterval = 0.1; // Delay between consecutive color checks in seconds
        private double elapsedColorCheckTime = 0;

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
            //Test
            List<UIElement> listElements = new();
            listElements.Add(PotHole_Obstacle_1);

            DriveManager.instance?.StartSim();
            Start_Sim_Button_Border.Visibility = Visibility.Collapsed;
            Stop_Sim_Button_Border.Visibility = Visibility.Visible;
            DriveManager.instance.heroCar = new HeroCar(HeroCar_Sprite, CarSpawner_Canvas_1);
            DriveManager.instance.civilianCar = new CivilianCar(PedCar_1_Sprite, CarSpawner_Canvas_1);
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            newRaycast = new(HeroCar_Sprite, ObstacleCanvas, 50);
            //rayCaster = new(HeroCar_Sprite, listElements);
            //raycastResult = new();
            //AssignMergedRoadImage(); After all these work, it wasn't even necessary *crying emoji*
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
            newRaycast.PerformRaycast();
            //Debug.WriteLine(raycastResult.Distance);

            double deltaTime = (e as RenderingEventArgs).RenderingTime.TotalSeconds - elapsedColorCheckTime;

            // Accumulate the elapsed time
            elapsedColorCheckTime += deltaTime;

            // Check if enough time has passed for the color check
            if (elapsedColorCheckTime >= colorCheckInterval)
            {
                // Update the color cache
                //UpdateColorCache(RoadCanvas, carCanvas);
                DriveManager.instance?.heroCar.Update();
                DriveManager.instance?.civilianCar.Update();

                // Reset the elapsed time
                elapsedColorCheckTime = 0;
            }




            // Update the car's position and perform other game logic

            //UpdateCarPosition();
        }
        public void UpdateCarPosition()
        {
            double carX = Canvas.GetLeft(HeroCar_Sprite);
            double carY = Canvas.GetTop(HeroCar_Sprite);
            //            UpdateColorCache();
            underlyingColor = GetUnderlyingColor();

            //// Check if the underlying color is green (grass)
            if (IsColorGreen(underlyingColor))
            {
                carMaxSpeed = 1;
                //Debug.WriteLine("Yes Color Found " + "R -" + underlyingColor.R + "    G -" + underlyingColor.G + "    B -" + underlyingColor.B);
                Debug.WriteLine("Yes Color Found ");
            }
            else
            {

                carMaxSpeed = 5;
                //Debug.WriteLine("NO, it is " + "R -" + underlyingColor.R + "    G -" + underlyingColor.G + "    B -" + underlyingColor.B);
                Debug.WriteLine("NO");
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
            RotateTransform rotation = new(angle);

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
        void UpdateColorCache()
        {

            Point carCanvasPosition = HeroCar_Sprite.TransformToVisual(RoadCanvas).Transform(new Point(0, 0));

            int carCanvasX = (int)carCanvasPosition.X;
            int carCanvasY = (int)carCanvasPosition.Y;

            if (carCanvasX >= 0 && carCanvasX < RoadCanvas.ActualWidth && carCanvasY >= 0 && carCanvasY < RoadCanvas.ActualHeight)
            {
                RenderTargetBitmap renderBitmap = new((int)RoadCanvas.ActualWidth, (int)RoadCanvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                renderBitmap.Render(RoadCanvas);

                CroppedBitmap croppedBitmap = new(renderBitmap, new Int32Rect(carCanvasX, carCanvasY, 1, 1));
                byte[] pixelColor = new byte[4];
                croppedBitmap.CopyPixels(pixelColor, 4, 0);

                if (pixelColor[2] == 60 && pixelColor[1] == 150 && pixelColor[0] == 60)
                {

                    underlyingColor = Color.FromArgb(255, 60, 150, 60);
                    //isColorCacheValid = false;
                }
                else
                {
                    underlyingColor = Color.Transparent;
                    //isColorCacheValid = false;

                }

                isColorCacheValid = true;
            }



        }



        // Get the color of the underlying image at the specified position in the given canvas
        Color GetUnderlyingColor()
        {
            if (!isColorCacheValid)
            {
                UpdateColorCache();
            }

            if (underlyingColor.R >= 50 && underlyingColor.R <= 70 &&
        underlyingColor.G >= 140 && underlyingColor.G <= 160 &&
        underlyingColor.B >= 50 && underlyingColor.B <= 70)
            {
                return underlyingColor; // Return the color if it falls within the range
            }
            else
            {
                return Color.Transparent; // Return transparent color if it doesn't fall within the range
            }
        }




        // Check if the color is green (grass)
        bool IsColorGreen(Color color)
        {
            return color.R == 60 && color.G == 150 && color.B == 60; // Adjust the color values as needed
        }


        RenderTargetBitmap CombineRoadCanvases()
        {
            RenderTargetBitmap renderBitmap = new((int)RoadCanvas.ActualWidth, (int)RoadCanvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            DrawingVisual drawingVisual = new();

            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                // Apply the rotation transformation to the DrawingContext
                RotateTransform rotationTransform = RoadCanvas.RenderTransform as RotateTransform;
                if (rotationTransform != null)
                {
                    drawingContext.PushTransform(rotationTransform);
                }

                foreach (Canvas roadCanvas in RoadCanvas.Children)
                {
                    // Apply the rotation transformation to the child canvas
                    RotateTransform childRotationTransform = roadCanvas.RenderTransform as RotateTransform;
                    if (childRotationTransform != null)
                    {
                        drawingContext.PushTransform(childRotationTransform);
                    }

                    Point roadCanvasPosition = roadCanvas.TransformToVisual(RoadCanvas).Transform(new Point(0, 0));
                    ImageSource backgroundImage = ((ImageBrush)roadCanvas.Background).ImageSource;

                    // Draw the background image onto the RenderTargetBitmap
                    drawingContext.DrawImage(backgroundImage, new Rect(roadCanvasPosition, roadCanvas.RenderSize));

                    // Restore the rotation transformation of the child canvas
                    if (childRotationTransform != null)
                    {
                        drawingContext.Pop();
                    }
                }

                // Restore the rotation transformation of the RoadCanvas
                if (rotationTransform != null)
                {
                    drawingContext.Pop();
                }
            }

            renderBitmap.Render(drawingVisual);

            return renderBitmap;
        }



        private void AssignMergedRoadImage()
        {

            RenderTargetBitmap mergedBitmap = CombineRoadCanvases();

            Image roadImage = new();
            roadImage.Source = mergedBitmap;
            // Remember to remove this
            //BitmapSaver(mergedBitmap, "");

            RoadCanvas.Children.Clear();
            RoadCanvas.Children.Add(roadImage);
        }

        public void BitmapSaver(RenderTargetBitmap renderTargetBitmap, string filePath)
        {
            //string filePath = "pack://application:,,,/NEATDrive_WPF;component/Resources/Images/Roads/SystemicRoads/Turn_Road.png";
            Uri uri = new(filePath, UriKind.Relative);

            // Create a PngBitmapEncoder
            PngBitmapEncoder encoder = new();

            // Create a MemoryStream to hold the image data
            using MemoryStream stream = new();
            // Save the RenderTargetBitmap to the MemoryStream
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            encoder.Save(stream);

            // Create a BitmapImage from the MemoryStream
            BitmapImage image = new();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();

            // Save the BitmapImage to the specified URI
            using var fileStream = new FileStream(uri.LocalPath, FileMode.Create);
            PngBitmapEncoder pngEncoder = new();
            pngEncoder.Frames.Add(BitmapFrame.Create(image));
            pngEncoder.Save(fileStream);



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
            //DriveManager.instance.heroCar = new HeroCar(HeroCar_Sprite, CarSpawner_Canvas);

            //DriveManager.instance.heroCar = new HeroCar(HeroCar_Sprite);

        }

        private void SimulationWindow1_Deactivated(object sender, EventArgs e)
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }


        private bool isDragging = false;
        private Point dragStartPosition;
        private double initialLeft;
        private double initialTop;

        // Event handler for mouse left button down event
        private void PotHole_Obstacle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Set the dragging flag and store the initial position
            isDragging = true;
            dragStartPosition = e.GetPosition(ParentCanvas);

            // Store the initial left and top positions of the PotHole_Obstacle canvas
            initialLeft = Canvas.GetLeft(PotHole_Obstacle_1);
            initialTop = Canvas.GetTop(PotHole_Obstacle_1);

            // Capture the mouse to the PotHole_Obstacle canvas
            PotHole_Obstacle_1.CaptureMouse();
        }

        // Event handler for mouse move event
        private void PotHole_Obstacle_MouseMove(object sender, MouseEventArgs e)
        {
            // Check if dragging is in progress
            if (isDragging)
            {
                // Get the current mouse position relative to the ParentCanvas
                Point currentPosition = e.GetPosition(ParentCanvas);

                // Calculate the offset from the drag start position
                double offsetX = currentPosition.X - dragStartPosition.X;
                double offsetY = currentPosition.Y - dragStartPosition.Y;

                // Update the position of the PotHole_Obstacle canvas based on the initial position and the offset
                Canvas.SetLeft(PotHole_Obstacle_1, initialLeft + offsetX);
                Canvas.SetTop(PotHole_Obstacle_1, initialTop + offsetY);
            }
        }

        // Event handler for mouse left button up event
        private void PotHole_Obstacle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Reset the dragging flag and release the captured mouse
            isDragging = false;
            PotHole_Obstacle_1.ReleaseMouseCapture();
        }



        // Event handler for MouseEnter event
        private void PotHole_MouseEnter(object sender, MouseEventArgs e)
        {
            // Set the MouseWheel event to be handled by the PotHole_Obstacle canvas
            PotHole_Obstacle_1.Focus();
        }

        // Event handler for MouseWheel event
        private void PotHole_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Get the scaling factor based on the mouse wheel delta
            double scaleFactor = e.Delta > 0 ? 1.1 : 0.9;
            //ScaleTransform scaleTransform = new(1.0, 1.0);
            //PotHole_Obstacle.RenderTransform = scaleTransform;

            if (PotHole_Obstacle_1.Width <= 130 && PotHole_Obstacle_1.Height <= 200
                && PotHole_Obstacle_1.Width >= 20 && PotHole_Obstacle_1.Height >= 30)
            {
                //scaleTransform.ScaleX = scaleFactor;
                // Scale the PotHole_Obstacle canvas
                PotHole_Obstacle_1.Width *= scaleFactor;
                PotHole_Obstacle_1.Height *= scaleFactor;
                //Debug.WriteLine(PotHole_Obstacle.Width + "  " + PotHole_Obstacle.Height);
            }
            else if (PotHole_Obstacle_1.Width > 130 || PotHole_Obstacle_1.Height > 200)
            {
                PotHole_Obstacle_1.Width = 130;
                PotHole_Obstacle_1.Height = 200;
            }
            else if (PotHole_Obstacle_1.Width < 20 || PotHole_Obstacle_1.Height < 30)
            {
                PotHole_Obstacle_1.Width = 20;
                PotHole_Obstacle_1.Height = 30;

            }

        }




    }
}
