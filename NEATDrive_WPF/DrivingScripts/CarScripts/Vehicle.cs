using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Color = System.Drawing.Color;

namespace NEATDrive_WPF.DrivingScripts.CarScripts
{
    abstract class Vehicle
    {
        protected Canvas? carCanvas;
        protected Canvas? RoadCanvas = ApplicationManager.instance?.simWindow?.RoadCanvas;
        protected bool isCarDrivable = false;
        protected Canvas spawnCanvas;


        protected double carSpeed;
        protected double carRotation;
        protected double carTurningSpeed = 3;
        protected double carFriction;
        protected double carPositionX;
        protected double carPositionY;
        protected double carMaxSpeed = 3.5;
        protected const double Acceleration = 0.05;
        protected const double Deceleration = 0.005;
        protected const double BrakingForce = 0.05;
        protected const double TurningAngle = 15;

        protected Color underlyingColor = Color.Transparent; // Default color
        protected bool isColorCacheValid = false;

        protected Grid rayContainer;

        //protected readonly Canvas canvas;
        //protected readonly System.Drawing.Rectangle carRect;


        public virtual void Update()
        {

        }

        public virtual void UpdateCarPosition()
        {

        }







        protected void UpdateColorCache(Canvas roadCanvas, Canvas carCanvas)
        {
            Point carCanvasPosition = carCanvas.TranslatePoint(new Point(0, 0), roadCanvas);

            int carCanvasX = (int)carCanvasPosition.X;
            int carCanvasY = (int)carCanvasPosition.Y;

            if (carCanvasX >= 0 && carCanvasX < roadCanvas.ActualWidth && carCanvasY >= 0 && carCanvasY < roadCanvas.ActualHeight)
            {
                RenderTargetBitmap renderBitmap = new((int)roadCanvas.ActualWidth, (int)roadCanvas.ActualHeight, 96d, 96d, System.Windows.Media.PixelFormats.Pbgra32);
                renderBitmap.Render(roadCanvas);

                CroppedBitmap croppedBitmap = new(renderBitmap, new Int32Rect(carCanvasX, carCanvasY, 1, 1));
                byte[] pixelColor = new byte[4];
                croppedBitmap.CopyPixels(pixelColor, 4, 0);

                if (pixelColor[2] >= 50 && pixelColor[2] <= 70 &&
                    pixelColor[1] >= 140 && pixelColor[1] <= 160 &&
                    pixelColor[0] >= 50 && pixelColor[0] <= 70)
                {
                    underlyingColor = Color.FromArgb(255, 60, 150, 60);
                    if (isColorCacheValid)
                    {
                        isColorCacheValid = false;
                    }
                }
                else
                {
                    underlyingColor = Color.White;
                    if (isColorCacheValid)
                    {
                        isColorCacheValid = false;
                    }
                }
            }
        }




        // Get the color of the underlying image at the specified position in the given canvas
        protected Color GetUnderlyingColor()
        {

            if (!isColorCacheValid)
            {
                UpdateColorCache(RoadCanvas, carCanvas);
            }

            return underlyingColor;
        }


        // Check if the color is green (grass)
        protected bool IsColorGreen(Color color)
        {
            return color.R == 60 && color.G == 150 && color.B == 60; // Adjust the color values as needed
        }
        private Rect GetRectOfObject(FrameworkElement _element)
        {
            //Rect rectangleBounds = _element.RenderTransform.TransformBounds(new Rect(_element.RenderSize));
            Rect rectangleBounds = _element.RenderTransform.TransformBounds(new Rect(0, 0, _element.ActualWidth, _element.ActualWidth));

            return rectangleBounds;
        }

        protected void DetectCollision()
        {
            // Get the bounding box of the HeroCar_Sprite canvas
            Rect carBounds = new(Canvas.GetLeft(carCanvas), Canvas.GetTop(carCanvas), carCanvas.ActualWidth, carCanvas.ActualHeight);
            //Rect carBounds = GetRectOfObject(carCanvas);

            // Get the bounding box of the PotHole_Obstacle canvas, replace with actual dynamic obstacle placements
            Rect potHoleBounds = new(Canvas.GetLeft(ApplicationManager.instance.simWindow.PotHole_Obstacle_1), Canvas.GetTop(ApplicationManager.instance.simWindow.PotHole_Obstacle_1), ApplicationManager.instance.simWindow.PotHole_Obstacle_1.ActualWidth, ApplicationManager.instance.simWindow.PotHole_Obstacle_1.ActualHeight);
            //Rect potHoleBounds = GetRectOfObject(ApplicationManager.instance.simWindow.PotHole_Obstacle_1);

            // Check if the bounding boxes intersect
            if (carBounds.IntersectsWith(potHoleBounds))
            {

                // Collision detected
                // Perform desired actions, such as hiding the HeroCar_Sprite canvas
                //ResetCar(carCanvas, spawnCanvas);
                //carCanvas.Visibility = Visibility.Hidden;
            }
        }

        protected void ResetCar(Canvas carSprite, Canvas spawnCanvas)
        {
            // In the case of NEAT, you must also add the Logic to restart the NEAT algo from beginning
            isCarDrivable = false;
            //double spawnerPositionX = Canvas.GetLeft(spawnCanvas);
            //double spawnerPositionY = Canvas.GetTop(spawnCanvas);
            System.Windows.Point spawnerPosition = new(Canvas.GetLeft(spawnCanvas), Canvas.GetTop(spawnCanvas));

            // Copy the position and rotation of the spawnCanvas to the carSprite
            Canvas.SetLeft(carSprite, spawnerPosition.X);
            Canvas.SetTop(carSprite, spawnerPosition.Y);
            carSprite.RenderTransform = spawnCanvas.RenderTransform;
            Debug.WriteLine("Car has reset");
        }


        protected virtual void Accelerate()
        {

        }

        protected virtual void Decelerate()
        {

        }

        protected virtual void TurnLeft()
        {

        }

        protected virtual void TurnRight()
        {

        }


        // Create Line elements for each ray
        Line frontRay = new() { Y1 = 5, Y2 = 5, X1 = 5, X2 = 5, Stroke = System.Windows.Media.Brushes.LightGray, StrokeThickness = 2, StrokeDashArray = new DoubleCollection(new double[] { 2.0, 1.0 }), Opacity = 0.5 };
        Line backRay = new() { Y1 = 5, Y2 = 5, X1 = 5, X2 = 5, Stroke = System.Windows.Media.Brushes.LightGray, StrokeThickness = 2, StrokeDashArray = new DoubleCollection(new double[] { 2.0, 1.0 }), Opacity = 0.5 };
        Line leftRay = new() { Y1 = 5, Y2 = 5, X1 = 5, X2 = 5, Stroke = System.Windows.Media.Brushes.LightGray, StrokeThickness = 2, StrokeDashArray = new DoubleCollection(new double[] { 2.0, 1.0 }), Opacity = 0.5 };
        Line rightRay = new() { Y1 = 5, Y2 = 5, X1 = 5, X2 = 5, Stroke = System.Windows.Media.Brushes.LightGray, StrokeThickness = 2, StrokeDashArray = new DoubleCollection(new double[] { 2.0, 1.0 }), Opacity = 0.5 };
        Line leftDiagonalRay = new() { Y1 = 5, Y2 = 5, X1 = 5, X2 = 5, Stroke = System.Windows.Media.Brushes.LightGray, StrokeThickness = 2, StrokeDashArray = new DoubleCollection(new double[] { 2.0, 1.0 }), Opacity = 0.5 };
        Line rightDiagonalRay = new() { Y1 = 5, Y2 = 5, X1 = 5, X2 = 5, Stroke = System.Windows.Media.Brushes.LightGray, StrokeThickness = 2, StrokeDashArray = new DoubleCollection(new double[] { 2.0, 1.0 }), Opacity = 0.5 };

        // Set initial properties for all rays
        System.Windows.Media.Color rayColor = System.Windows.Media.Colors.Red;
        double rayWidth = 1.0; // Initial width of the rays
        double rayLength = 10.0; // Maximum length of the rays


        protected virtual void RenderRays()
        {
            UpdateRays();
            // Set common properties for all rays
            foreach (Line ray in new[] { frontRay, backRay, leftRay, rightRay, leftDiagonalRay, rightDiagonalRay })
            {
                ray.Stroke = new System.Windows.Media.SolidColorBrush(rayColor);
                ray.StrokeThickness = rayWidth;
            }

        }

        // Update the rays' positions and directions based on the car's position and orientation
        void UpdateRays()
        {
            // Calculate the starting positions of each ray based on the car's position
            System.Windows.Point carPosition = new(Canvas.GetLeft(carCanvas), Canvas.GetTop(carCanvas));
            System.Windows.Point frontRayStart = new(carPosition.X + carCanvas.Width / 2, carPosition.Y + carCanvas.Height / 2);
            System.Windows.Point backRayStart = new(carPosition.X + carCanvas.Width / 2, carPosition.Y);
            System.Windows.Point leftRayStart = new(carPosition.X, carPosition.Y + carCanvas.Height / 2);
            System.Windows.Point rightRayStart = new(carPosition.X + carCanvas.Width, carPosition.Y + carCanvas.Height / 2);
            System.Windows.Point leftDiagonalRayStart = new(carPosition.X, carPosition.Y);
            System.Windows.Point rightDiagonalRayStart = new(carPosition.X + carCanvas.Width, carPosition.Y);

            // Calculate the directions of each ray based on the car's orientation
            //double carRotation = carRotation; // Implement a method to get the car's rotation in degrees
            Vector frontRayDirection = new(Math.Cos(carRotation * Math.PI / 180), Math.Sin(carRotation * Math.PI / 180));
            Vector backRayDirection = new(Math.Cos((carRotation + 180) * Math.PI / 180), Math.Sin((carRotation + 180) * Math.PI / 180));
            Vector leftRayDirection = new(Math.Cos((carRotation - 90) * Math.PI / 180), Math.Sin((carRotation - 90) * Math.PI / 180));
            Vector rightRayDirection = new(Math.Cos((carRotation + 90) * Math.PI / 180), Math.Sin((carRotation + 90) * Math.PI / 180));
            Vector leftDiagonalRayDirection = new(Math.Cos((carRotation - 45) * Math.PI / 180), Math.Sin((carRotation - 45) * Math.PI / 180));
            Vector rightDiagonalRayDirection = new(Math.Cos((carRotation + 45) * Math.PI / 180), Math.Sin((carRotation + 45) * Math.PI / 180));

            // Calculate the endpoints of each ray based on the starting positions and directions
            System.Windows.Point frontRayEnd = CalculateRayEndpoint(frontRayStart, frontRayDirection);
            System.Windows.Point backRayEnd = CalculateRayEndpoint(backRayStart, backRayDirection);
            System.Windows.Point leftRayEnd = CalculateRayEndpoint(leftRayStart, leftRayDirection);
            System.Windows.Point rightRayEnd = CalculateRayEndpoint(rightRayStart, rightRayDirection);
            System.Windows.Point leftDiagonalRayEnd = CalculateRayEndpoint(leftDiagonalRayStart, leftDiagonalRayDirection);
            System.Windows.Point rightDiagonalRayEnd = CalculateRayEndpoint(rightDiagonalRayStart, rightDiagonalRayDirection);

            // Update the endpoints of each ray
            frontRay.X1 = frontRayStart.X;
            frontRay.Y1 = frontRayStart.Y;
            frontRay.X2 = frontRayEnd.X;
            frontRay.Y2 = frontRayEnd.Y;

            backRay.X1 = backRayStart.X;
            backRay.Y1 = backRayStart.Y;
            backRay.X2 = backRayEnd.X;
            backRay.Y2 = backRayEnd.Y;

            leftRay.X1 = leftRayStart.X;
            leftRay.Y1 = leftRayStart.Y;
            leftRay.X2 = leftRayEnd.X;
            leftRay.Y2 = leftRayEnd.Y;

            rightRay.X1 = rightRayStart.X;
            rightRay.Y1 = rightRayStart.Y;
            rightRay.X2 = rightRayEnd.X;
            rightRay.Y2 = rightRayEnd.Y;

            leftDiagonalRay.X1 = leftDiagonalRayStart.X;
            leftDiagonalRay.Y1 = leftDiagonalRayStart.Y;
            leftDiagonalRay.X2 = leftDiagonalRayEnd.X;
            leftDiagonalRay.Y2 = leftDiagonalRayEnd.Y;

            rightDiagonalRay.X1 = rightDiagonalRayStart.X;
            rightDiagonalRay.Y1 = rightDiagonalRayStart.Y;
            rightDiagonalRay.X2 = rightDiagonalRayEnd.X;
            rightDiagonalRay.Y2 = rightDiagonalRayEnd.Y;

            // Add the ray Line elements to the canvas or container where you want to display them
            rayContainer.Children.Add(frontRay);
            rayContainer.Children.Add(backRay);
            rayContainer.Children.Add(leftRay);
            rayContainer.Children.Add(rightRay);
            rayContainer.Children.Add(leftDiagonalRay);
            rayContainer.Children.Add(rightDiagonalRay);


        }








        // Calculate the endpoint of a ray based on the starting position and direction
        System.Windows.Point CalculateRayEndpoint(System.Windows.Point startPoint, Vector direction)
        {
            double rayLength = 10.0; // Adjust this value to the desired maximum length of the rays
            System.Windows.Point endPoint = new(startPoint.X + direction.X * rayLength, startPoint.Y + direction.Y * rayLength);
            return endPoint;
        }


    }
}
