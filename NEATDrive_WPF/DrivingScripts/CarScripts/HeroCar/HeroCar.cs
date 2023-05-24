using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace NEATDrive_WPF.DrivingScripts.CarScripts.HeroCar
{
    class HeroCar : Vehicle
    {
        Canvas carCanvas;

        public HeroCar(Canvas canvas)
        {
            carCanvas = canvas;

            x = canvas.ActualWidth / 2;
            y = canvas.ActualHeight / 2;
            speed = 0;
            angle = 0;

            /*
            // Initialize the car's position
            x = carCanvas.ActualWidth / 2;
            y = carCanvas.ActualHeight / 2;

            // Set the initial speed and angle
            speed = 0;
            angle = 0;
            */

        }
        public void Update()
        {
            //CarMove();
            //Debug.WriteLine("Jaara na Aage lode!");
            //Debug.WriteLine(speed);
            // Update the car's position based on speed and angle

            x += speed;

            // Move the car's canvas using Canvas.SetLeft method
            Canvas.SetLeft(carCanvas, x);

            // Check if the car has reached the window's right edge
            if (x > carCanvas.ActualWidth)
            {
                // Reset the car's position
                x = -carCanvas.Width;
            }



            double angleInRadians = angle * Math.PI / 180;
            x += speed * Math.Sin(angleInRadians);
            y -= speed * Math.Cos(angleInRadians);

            // Keep the car within the window limits
            //double windowWidth = ApplicationManager.instance.simWindow.ActualWidth;
            //double windowHeight = ApplicationManager.instance.simWindow.ActualHeight;
            double windowWidth = 2; //= ApplicationManager.instance.simWindow.SimGrid.ActualWidth;
            double windowHeight = 2;// = ApplicationManager.instance.simWindow.SimGrid.ActualHeight;
            double carWidth = carCanvas.ActualWidth;
            double carHeight = carCanvas.ActualHeight;

            x = Math.Min(Math.Max(x, carWidth / 2), windowWidth - carWidth / 2);
            y = Math.Min(Math.Max(y, carHeight / 2), windowHeight - carHeight / 2);

            // Update the car's position on the canvas
            Canvas.SetLeft(carCanvas, x - carWidth / 2);
            Canvas.SetTop(carCanvas, y - carHeight / 2);
            Debug.WriteLine("This is SetLeft Value" + (x - carWidth / 2));
            Debug.WriteLine("This is SetTop Value" + (y - carHeight / 2));


            // Limit the speed to the maximum speed
            if (speed > MaxSpeed)
            {
                speed = MaxSpeed;
            }


            // Decelerate the car when no acceleration key is pressed
            if (Math.Abs(speed) > 0 && !IsAccelerationKeyDown())
            {
                speed -= Math.Sign(speed) * Deceleration;
                if (Math.Abs(speed) < Deceleration)
                {
                    speed = 0;
                }

            }

        }

        void CarMove()
        {
            double angleInRadians = angle * Math.PI / 180;
            x += speed * Math.Sin(angleInRadians);
            y -= speed * Math.Cos(angleInRadians);

            // Keep the car within the window limits
            double windowWidth = 2; //ApplicationManager.instance.simWindow.SimGrid.ActualWidth;
            double windowHeight = 2;// ApplicationManager.instance.simWindow.SimGrid.ActualHeight;
            double carWidth = carCanvas.ActualWidth;
            double carHeight = carCanvas.ActualHeight;

            x = Math.Min(Math.Max(x, carWidth / 2), windowWidth - carWidth / 2);
            y = Math.Min(Math.Max(y, carHeight / 2), windowHeight - carHeight / 2);

            // Calculate the position of the car within the grid's canvas children
            double columnWidth = windowWidth / 2; //ApplicationManager.instance.simWindow.SimGrid.ColumnDefinitions.Count;
            double rowHeight = windowHeight / 2;// ApplicationManager.instance.simWindow.SimGrid.RowDefinitions.Count;
            int column = (int)(x / columnWidth);
            int row = (int)(y / rowHeight);

            // Update the car's position within the grid's canvas children
            Canvas.SetLeft(ApplicationManager.instance.simWindow.HeroCar_Sprite, column * columnWidth);
            Canvas.SetTop(ApplicationManager.instance.simWindow.HeroCar_Sprite, row * rowHeight);

            // Limit the speed to the maximum speed
            if (speed > MaxSpeed)
                speed = MaxSpeed;

            // Decelerate the car when no acceleration key is pressed
            if (Math.Abs(speed) > 0 && !IsAccelerationKeyDown())
            {
                speed -= Math.Sign(speed) * Deceleration;
                if (Math.Abs(speed) < Deceleration)
                    speed = 0;
            }
        }

        void CarMove2()
        {
            double angleInRadians = angle * Math.PI / 180;
            x += speed * Math.Sin(angleInRadians);
            y -= speed * Math.Cos(angleInRadians);

            // Set the updated position of the car on the canvas
            //Canvas.SetLeft(ApplicationManager.instance.simWindow.HeroCar_Sprite, x);
            //Canvas.SetTop(ApplicationManager.instance.simWindow.HeroCar_Sprite, y);
            Debug.WriteLine(y);
        }

        public void Accelerate()
        {
            speed += Acceleration;

        }

        public void Decelerate()
        {
            speed -= Deceleration;
        }

        public void TurnLeft()
        {
            angle -= TurnAngle;
        }

        public void TurnRight()
        {
            angle += TurnAngle;
        }

        private bool IsAccelerationKeyDown()
        {
            return Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.S);
        }
    }
}
