using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace NEATDrive_WPF.DrivingScripts
{
    /// <summary>
    /// This class contains the master controls of the driving sim guys
    /// </summary>

    class DriveManager
    {
        

        public DispatcherTimer simTimer = new();
        List<Rectangle> itemRemover = new();

        ConfigurationWindow simWindow;

        Random rand= new();

        ImageBrush playerImage = new();
        ImageBrush starImage = new();

        Rect playerHitBox;

        double carSpeed = 0;
        double carAcceleration = 0.25;
        double carMaxSpeed = 5;
        double carFriction = 0.02;
        double carRotation = 0;
        double carPositionX=0;
        double carPositionY=0;
        double carTurningSpeed = 4;

        int playerSpeed = 10;
        int carNum;
        int starCounter = 30;
        int powerModeCounter = 200;
        
        //Text scoreText;

        double score;
        double i;

        bool simStart=true,isAccelerating, isBraking, isTurningLeft, isTurningRight, simOver, powerMode;
        
        public DriveManager(ConfigurationWindow windowToRunSimOn)
        {
            simWindow=windowToRunSimOn;
        }

        public bool isSimStart()
        {
            return simStart;
        }
        public void StartSim()
        {
            //carSpeed = 8;
            simTimer.Start();
            //isTurningLeft = false;
            //isTurningRight = false;
            //simOver = false;
            //powerMode = false;

            //score = 0;


            playerImage.ImageSource = new BitmapImage(new Uri("D:\\Comp Engg Files\\BE-A Sem II\\Project\\C# Version\\NEAT-master\\NeatTest\\NEATDrive_WPF\\Resources\\Images\\Props\\Sports_car.png"));

            simWindow.player.Fill = playerImage;

            //foreach (var item in itemRemover) 
            //{

            //}
        
        }

        public void InitTimer()
        {
            simTimer.Tick += simWindow.SimLoop;
            simTimer.Interval = TimeSpan.FromMilliseconds(16.66);
        }

        public void StopSim()
        {

        }

        #region DrivePhysics
        public void UpdateCarPosition()
        {
            if(isTurningLeft && carSpeed>=1)
            {
                carRotation -= 1 * carTurningSpeed;
            }
            if(isTurningRight && carSpeed >= 1)
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

        }
        #endregion


    }

}
