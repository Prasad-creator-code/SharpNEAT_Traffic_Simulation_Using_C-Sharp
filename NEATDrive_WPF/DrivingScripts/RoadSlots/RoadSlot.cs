using System;
using System.Windows.Media.Imaging;

namespace NEATDrive_WPF.DrivingScripts.RoadSlots
{
    public class RoadSlot
    {
        public BitmapImage? SelectedImage { get; set; }
        public double currentRotationAngle { get; set; }

        public RoadSlot(Uri imageUri)
        {
            SelectedImage = new BitmapImage(imageUri);
        }
        //public RoadConfiguration Road_Config { get; set; }
    }
}
