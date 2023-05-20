using System.Drawing;
using System.Windows.Controls;

namespace NEATDrive_WPF.DrivingScripts.CarScripts
{
    abstract class Vehicle
    {
        protected const double Acceleration = 0.1;
        protected const double Deceleration = 0.2;
        protected const double MaxSpeed = 5;
        protected const double TurnAngle = 4;

        protected readonly Canvas canvas;
        protected readonly Rectangle carRect;

        protected double x;
        protected double y;
        protected double speed;
        protected double angle;

        protected bool isAccelerating;
        protected bool isTurningLeft;
        protected bool isTurningRight;
    }
}
