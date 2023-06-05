using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NEATDrive_WPF.DrivingScripts.CarScripts.CivilianCar
{
    public class CivilianCar : Vehicle
    {

        public CivilianCar(Canvas canvas, Canvas spawnTransform, Canvas destCanvas)
        {

            carCanvas = canvas;
            spawnCanvas = spawnTransform;
            rayContainer = new Grid();
            carCanvas.Children.Add(rayContainer);
            destinationCanvas = destCanvas;
        }
        public override void Update()
        {
            base.Update();
            UpdateCarPosition();

        }
        public override void UpdateCarPosition()
        {
            base.UpdateCarPosition();

            //RenderRays();

            //Debug.WriteLine("Civilian Car Update");
            if (CRASHED)
            {
                CRASHED = false;
                CrashTimes += 1;
            }
            if (REACHED)
            {
                REACHED = false;
                ReachedTimes += 1;
            }
            double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
            SimulationManager.instance.CycleStats(elapsedSeconds, ReachedTimes, 0, (int)elapsedSeconds + 3 - 7, CrashTimes);

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
                if (carMaxSpeed != 2)
                {
                    carMaxSpeed = 2;
                }

                //Debug.WriteLine("NO, it is " + "R -" + underlyingColor.R + "    G -" + underlyingColor.G + "    B -" + underlyingColor.B);
                //Debug.WriteLine("NO");
            }

            //TESTDRIVE
            if (carSpeed > 0 || carSpeed < 0)
            {
                //carRotation -= carTurningSpeed;
                // PUT ROTATION HERE
                //RotateRight();
                //carRotation =;

            }
            TurnAwayFromDetectedObject();
            double carDirectionX = Math.Cos(carRotation * Math.PI / 180);
            double carDirectionY = Math.Sin(carRotation * Math.PI / 180);

            //TESTDRIVE
            //carSpeed += 0.01;
            if (carSpeed > carMaxSpeed)
            {
                carSpeed = carMaxSpeed;
            }
            else
            {
                MoveForward();
            }




            carPositionX += carDirectionX * carSpeed;
            carPositionY += carDirectionY * carSpeed;


            Canvas.SetLeft(carCanvas, carPositionX);
            Canvas.SetTop(carCanvas, carPositionY);

            double angle = Math.Atan2(carDirectionY, carDirectionX) + Math.PI / 2;
            angle *= 180 / Math.PI;
            angle = Math.Round(angle, 2);
            RotateTransform rotation = new(angle);

            carCanvas.RenderTransform = rotation;


        }

        public double GetSpeed()
        {
            return 0;// carSpeed;
        }

        public double GetRotation()
        {
            return 0;// carRotation;
        }


        public void MoveForward(double acceleration = 0)
        {
            carSpeed += Acceleration;
        }

        public void Brake(double acceleration = 0)
        {
            carSpeed -= Acceleration;
        }

        public void RotateRight(double carTurnSpeed = 0)
        {
            carRotation += carTurningSpeed;
        }

        public void RotateLeft(double carTurnSpeed = 0)
        {
            carRotation -= carTurningSpeed;
        }



        public void TurnAwayFromDetectedObject()
        {
            if (ApplicationManager.instance.simWindow.newRaycast.closestElement == carDestinationCanvas)
            {
                ResetCar(carCanvas, spawnCanvas);
            }
            if (ApplicationManager.instance.simWindow.newRaycast.closestElement == null)
            {
                carRotation = 90;
                return;
            }
            Vector direction = ApplicationManager.instance.simWindow.newRaycast.closestElementDirection;



            if (direction.X < 0)
            {
                //if (hasResetRotation)
                //{
                //    hasResetRotation = false;
                //}

                // Object detected to the left, execute turn right
                RotateRight();
                Debug.WriteLine("Turn Right");
            }
            else if (direction.X > 0)
            {
                //if (hasResetRotation)
                //{
                //    hasResetRotation = false;
                //}
                // Object detected to the right, execute turn left
                RotateLeft();
                Debug.WriteLine("Turn Left");
            }
            else
            {
                //if (!hasResetRotation)
                //{
                //    //carRotation = 0;
                //    hasResetRotation = true;
                //}

                //Debug.WriteLine("No turn needed");
                // No object detected or object is directly ahead, no turn needed
                ContinueTowardsDestination();

            }
        }

        bool hasResetRotation = false;
        FrameworkElement carDestinationCanvas = ApplicationManager.instance.simWindow.CarDestination_Canvas_1;


        public void ContinueTowardsDestination()
        {
            Debug.WriteLine("Continue Towards Destination");
            // Get the position of the carDestinationCanvas relative to the carCanvas
            Point carDestinationPosition = carDestinationCanvas.TranslatePoint(new Point(0, 0), carCanvas);

            // Pass the carDestinationPosition to the DriveTowardsCarDestination method
            DriveTowardsCarDestination(carDestinationPosition, carRotation);
        }






        public void DriveTowardsCarDestination(Point carDestinationPosition, double carRotation)
        {
            // Get the center position of the carCanvas
            Point carCenter = new(carCanvas.ActualWidth / 2, carCanvas.ActualHeight / 2);

            // Calculate the direction vector from the carCenter to the carDestinationPosition
            Vector direction = carDestinationPosition - carCenter;

            // Normalize the direction vector
            //direction.Normalize();

            // Calculate the angle between the current carRotation and the desired direction
            double angle = Vector.AngleBetween(new Vector(Math.Cos(carRotation), Math.Sin(carRotation)), direction);

            // Determine the rotation direction based on the angle
            if (angle < 0)
            {
                // Rotate left
                RotateLeft();
            }
            else
            {
                // Rotate right
                RotateRight();
            }

        }




        public double Intelligence()
        {
            Vector destinationDirection = PerformRaycast();
            double angle = Vector.AngleBetween(new Vector(1, 0), destinationDirection);
            return angle;
        }

        private Vector PerformRaycast()
        {
            // Get the center position of the carCanvas
            Point carCenter = new(carCanvas.ActualWidth / 2, carCanvas.ActualHeight / 2);

            // Calculate the direction from the car to the destinationCanvas
            Vector direction = Point.Subtract(destinationCanvas.TranslatePoint(new Point(0, 0), carCanvas), carCenter);
            direction.Normalize();

            return direction;
        }

        private double AdjustRotationTowardsDestination(Canvas car, Canvas destination, double currentRotation)
        {
            // Get the center position of the carCanvas
            Point carCenter = new(car.ActualWidth / 2, car.ActualHeight / 2);

            // Calculate the direction from the car to the destinationCanvas
            Vector direction = Point.Subtract(destination.TranslatePoint(new Point(0, 0), car), carCenter);
            direction.Normalize();

            // Calculate the angle between the current rotation and the direction angle
            double destinationAngle = Math.Atan2(direction.Y, direction.X) * (180 / Math.PI);
            double angleDiff = destinationAngle - currentRotation;

            // Adjust the angle difference to be within -180 to 180 range
            if (angleDiff > 180)
                angleDiff -= 360;
            else if (angleDiff < -180)
                angleDiff += 360;

            // Adjust the rotation based on the angle difference
            double rotationSpeed = 1.0; // Adjust the rotation speed as needed
            double newRotation = currentRotation + (rotationSpeed * angleDiff);

            // Normalize the new rotation to be within 0 to 360 range
            if (newRotation < 0)
                newRotation += 360;
            else if (newRotation > 360)
                newRotation -= 360;

            return newRotation;
        }


    }
}
