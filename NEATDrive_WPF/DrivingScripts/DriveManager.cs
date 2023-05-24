using NEATDrive_WPF.DrivingScripts.CarScripts.HeroCar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace NEATDrive_WPF.DrivingScripts
{
    /// <summary>
    /// This class contains the master controls of the driving sim guys
    /// </summary>

    class DriveManager
    {
        public static DriveManager? instance;

        public DispatcherTimer simTimer = new();
        List<Rectangle> itemRemover = new();

        Random rand = new();

        ImageBrush playerImage = new();
        ImageBrush starImage = new();

        Rect playerHitBox;
        bool isColliding;

        double carSpeed = 0;
        double carAcceleration = 0.25;
        double carMaxSpeed = 5;
        double carFriction = 0.02;
        double carRotation = 0;
        double carPositionX = 0;
        double carPositionY = 0;
        double carTurningSpeed = 4;

        int playerSpeed = 10;
        int carNum;
        int starCounter = 30;
        int powerModeCounter = 200;

        //Text scoreText;

        double score;
        double i;

        bool simStart = true, isAccelerating, isBraking, isTurningLeft, isTurningRight, simOver, powerMode;

        public HeroCar heroCar;

        public bool isSimStart()
        {
            return simStart;
        }
        public void StartSim()
        {
            //carSpeed = 8;
            simTimer.Start();
            InitTimer();
            //var car = new HeroCar(ApplicationManager.instance.simWindow.HeroCar_Sprite, new Rectangle());
            //isTurningLeft = false;
            //isTurningRight = false;
            //simOver = false;
            //powerMode = false;
            //score = 0;

            //foreach (var item in itemRemover) 
            //{

            //}

        }

        public void InitTimer()
        {
            simTimer.Tick += ApplicationManager.instance.simWindow.SimLoop;
            simTimer.Interval = TimeSpan.FromMilliseconds(16.66);
        }

        public void StopSim()
        {
            simTimer.Stop();
        }

        #region DrivePhysics
        /*
        List<Point> roadPolygon = new List<Point>()
        {
            new Point(0, 0),
            new Point(100, 0),
            new Point(100, 50),
            new Point(0, 50)
        };
        private bool IsInsidePolygon(Point point, List<Point> polygon)
        {
            bool inside = false;
            int j = polygon.Count - 1;

            for (int i = 0; i < polygon.Count; i++)
            {
                if (((polygon[i].Y <= point.Y && point.Y < polygon[j].Y) || (polygon[j].Y <= point.Y && point.Y < polygon[i].Y)) &&
                    (point.X > (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    inside = !inside;
                }

                j = i;
            }

            return inside;
        }
        private Point GetRandomPositionOnRoad()
        {
            Random rand = new Random();
            Point randomPoint = new Point(rand.Next(0, 100), rand.Next(0, 50));

            while (!IsInsidePolygon(randomPoint, roadPolygon))
            {
                randomPoint = new Point(rand.Next(0, 100), rand.Next(0, 50));
            }

            return randomPoint;
        }
        */
        /*private bool IsColliding(double x, double y, double width, double height)
        {
            Rect carRect = new Rect(x, y, width, height);
            Rect collisionRect = new Rect(simWindow.CollisionRect.RadiusX, simWindow.CollisionRect.RadiusY, simWindow.CollisionRect.Width, simWindow.CollisionRect.Height);

            if (carRect.IntersectsWith(collisionRect))
            {
                return true;
            }

            return false;
        }

        public void UpdateCarPosition()
        {
            // Get current position of car
            double carLeft = Canvas.GetLeft(simWindow.player);
            double carTop = Canvas.GetTop(simWindow.player);

            // Check if car is within boundaries of road
            if (IsColliding(carLeft, carTop, simWindow.player.Width, simWindow.player.Height))
            {
                // If car is offroad, reset its position to a random point on the road
                Point roadPoint = GetRandomPositionOnRoad();
                Canvas.SetLeft(simWindow.player, roadPoint.X);
                Canvas.SetTop(simWindow.player, roadPoint.Y);
            }
            else
            {
                // If car is on the road, update its position as usual
                if (isTurningLeft && carSpeed >= 1)
                {
                    carRotation -= 1 * carTurningSpeed;
                }
                if (isTurningRight && carSpeed >= 1)
                {
                    carRotation += 1 * carTurningSpeed;
                }
                if (isAccelerating)
                {

                    carSpeed += carAcceleration;
                    if (carSpeed > carMaxSpeed)
                    {
                        carSpeed = carMaxSpeed;
                    }
                }
                else if (isBraking)
                {
                    carSpeed -= carAcceleration;
                    if (carSpeed < 0)
                    {
                        carSpeed = 0;
                    }
                }
                else
                {
                    carSpeed -= carFriction;
                    if (carSpeed < 0)
                    {
                        carSpeed = 0;
                    }
                }

                double carDirectionX = Math.Cos(carRotation * Math.PI / 180);
                double carDirectionY = Math.Sin(carRotation * Math.PI / 180);

                carPositionX += carDirectionX * carSpeed;
                carPositionY += carDirectionY * carSpeed;

                Canvas.SetLeft(simWindow.player, carPositionX);
                Canvas.SetTop(simWindow.player, carPositionY);

                double angle = Math.Atan2(carDirectionY, carDirectionX) + Math.PI / 2;
                angle *= 180 / Math.PI;
                angle = Math.Round(angle, 2);
                RotateTransform rotation = new RotateTransform(angle);
                simWindow.player.RenderTransform = rotation;

            }
            
        }
        #endregion

        #region Control Parameters
        public void Accelerate()
        {
            isAccelerating=true;
        }
        void Brake()
        {
            isBraking = true;
        }
        void TurnLeft()
        {
            isTurningLeft = true;
            
        }
        void TurnRight()
        {
            isTurningRight = true;
            
        }
        internal void DirectionalDrivePress(KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Up:
                    Accelerate();
                    break;
                case Key.Down:
                    Brake();
                    break;
                case Key.Left:
                    TurnLeft();
                    break;
                case Key.Right:
                    TurnRight();
                    break;

            }
            
        }

        internal void DirectionalDriveRelease(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    isAccelerating = false;
                    break;
                case Key.Down:
                    isBraking=false;
                    break;
                case Key.Left:
                    isTurningLeft=false;
                    break;
                case Key.Right:
                    isTurningRight=false;
                    break;

            }

        }*/
        #endregion


    }

}
