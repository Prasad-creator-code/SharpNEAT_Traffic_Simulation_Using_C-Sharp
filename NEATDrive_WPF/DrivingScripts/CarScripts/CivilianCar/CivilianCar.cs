using System;
using System.Windows.Controls;

namespace NEATDrive_WPF.DrivingScripts.CarScripts.CivilianCar
{
    class CivilianCar : Vehicle
    {
        public CivilianCar(Canvas canvas, Canvas spawnTransform)
        {

            carCanvas = canvas;
            spawnCanvas = spawnTransform;
            rayContainer = new Grid();
            carCanvas.Children.Add(rayContainer);

        }
        public override void Update()
        {
            base.Update();
            //UpdateCarPosition();

        }
        public override void UpdateCarPosition()
        {
            base.UpdateCarPosition();

            //RenderRays();

            //Debug.WriteLine("Civilian Car Update");


            DetectCollision();

            if (!isCarDrivable)
            {
                // Spawn the car at the CarSpawner position
                carPositionX = Canvas.GetLeft(spawnCanvas);
                carPositionY = Canvas.GetTop(spawnCanvas);

                // Set the car's position and rotation
                Canvas.SetLeft(carCanvas, carPositionX);
                Canvas.SetTop(carCanvas, carPositionY);
                carCanvas.RenderTransform = spawnCanvas.RenderTransform;

                isCarDrivable = true;
                return;
            }

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

            if (carSpeed > 0 || carSpeed < 0)
            {
                carRotation -= carTurningSpeed;
            }

            double carDirectionX = Math.Cos(carRotation * Math.PI / 180);
            double carDirectionY = Math.Sin(carRotation * Math.PI / 180);


            carSpeed += Acceleration;

            carPositionX += carDirectionX * carSpeed;
            carPositionY += carDirectionY * carSpeed;


            Canvas.SetLeft(carCanvas, carPositionX);
            Canvas.SetTop(carCanvas, carPositionY);

            //double angle = Math.Atan2(carDirectionY, carDirectionX) + Math.PI / 2;
            //angle *= 180 / Math.PI;
            //angle = Math.Round(angle, 2);
            //RotateTransform rotation = new(angle);

            //carCanvas.RenderTransform = rotation;


        }
    }
}
