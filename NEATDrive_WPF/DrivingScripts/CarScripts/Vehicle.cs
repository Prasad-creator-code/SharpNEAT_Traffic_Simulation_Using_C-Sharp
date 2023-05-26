using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using Point = System.Windows.Point;

namespace NEATDrive_WPF.DrivingScripts.CarScripts
{
    abstract class Vehicle
    {
        protected Canvas carCanvas;
        protected Canvas RoadCanvas = ApplicationManager.instance?.simWindow.RoadCanvas;


        protected double carSpeed;
        protected double carRotation;
        protected double carTurningSpeed = 2.5;
        protected double carFriction;
        protected double carPositionX;
        protected double carPositionY;
        protected double carMaxSpeed = 5;
        protected const double Acceleration = 0.25;
        protected const double Deceleration = 0.01;
        protected const double BrakingForce = 0.1;
        protected const double TurningAngle = 15;

        protected Color underlyingColor = Color.Transparent; // Default color
        protected bool isColorCacheValid = false;

        protected readonly Canvas canvas;
        protected readonly Rectangle carRect;

        protected void UpdateColorCache(Canvas roadCanvas, Canvas carCanvas)
        {
            Point carCanvasPosition = carCanvas.TranslatePoint(new Point(0, 0), roadCanvas);

            int carCanvasX = (int)carCanvasPosition.X;
            int carCanvasY = (int)carCanvasPosition.Y;

            if (carCanvasX >= 0 && carCanvasX < roadCanvas.ActualWidth && carCanvasY >= 0 && carCanvasY < roadCanvas.ActualHeight)
            {
                RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)roadCanvas.ActualWidth, (int)roadCanvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                renderBitmap.Render(roadCanvas);

                CroppedBitmap croppedBitmap = new CroppedBitmap(renderBitmap, new Int32Rect(carCanvasX, carCanvasY, 1, 1));
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

    }
}
