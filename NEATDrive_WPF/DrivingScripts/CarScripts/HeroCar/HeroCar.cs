using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NEATDrive_WPF.DrivingScripts.CarScripts.HeroCar
{
    class HeroCar : Vehicle
    {

        public HeroCar(Canvas canvas)
        {
            carCanvas = canvas;

        }
        public void Update()
        {
            UpdateCarPosition();

        }

        public void UpdateCarPosition()
        {
            double carX = Canvas.GetLeft(carCanvas);
            double carY = Canvas.GetTop(carCanvas);
            //            UpdateColorCache();
            underlyingColor = GetUnderlyingColor();

            //// Check if the underlying color is green (grass)
            if (IsColorGreen(underlyingColor))
            {
                if (carMaxSpeed != 1)
                {
                    carMaxSpeed = 1;
                }


                //Debug.WriteLine("Yes Color Found " + "R -" + underlyingColor.R + "    G -" + underlyingColor.G + "    B -" + underlyingColor.B);
                //Debug.WriteLine("Yes Color Found ");
            }
            else
            {
                if (carMaxSpeed != 5)
                {
                    carMaxSpeed = 5;
                }

                //Debug.WriteLine("NO, it is " + "R -" + underlyingColor.R + "    G -" + underlyingColor.G + "    B -" + underlyingColor.B);
                //Debug.WriteLine("NO");
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
                carSpeed -= BrakingForce;
                if (carSpeed <= 0)
                {
                    carSpeed = -carMaxSpeed / 2;
                }
            }



            double carDirectionX = Math.Cos(carRotation * Math.PI / 180);
            double carDirectionY = Math.Sin(carRotation * Math.PI / 180);

            carPositionX += carDirectionX * carSpeed;
            carPositionY += carDirectionY * carSpeed;

            Canvas.SetLeft(carCanvas, carPositionX);
            Canvas.SetTop(carCanvas, carPositionY);

            double angle = Math.Atan2(carDirectionY, carDirectionX) + Math.PI / 2;
            angle *= 180 / Math.PI;
            angle = Math.Round(angle, 2);
            RotateTransform rotation = new(angle);

            carCanvas.RenderTransform = rotation;





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


            }
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
    }
}
