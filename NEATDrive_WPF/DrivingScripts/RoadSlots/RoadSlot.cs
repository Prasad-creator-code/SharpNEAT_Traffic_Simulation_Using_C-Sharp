using System;
using System.Windows.Media.Imaging;

namespace NEATDrive_WPF.DrivingScripts.RoadSlots
{
    public class RoadSlot
    {
        public BitmapImage SelectedImage = new BitmapImage(new Uri("pack://application:,,,/NEATDrive_WPF;component/Resources/Images/Props/Grass_Cute.png", UriKind.Absolute));
        public Uri publicImageUri { get; set; }
        public double currentRotationAngle { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public RoadSlot(Uri imageUri, int row, int column)
        {
            publicImageUri = imageUri;
            SelectedImage = new BitmapImage(publicImageUri);
            Row = row;
            Column = column;
        }

        public override bool Equals(object obj)
        {
            if (obj is RoadSlot other)
            {
                // Customize the comparison logic based on your requirements
                // Return true if the instances are considered equal
                // For example:
                return this.Row == other.Row && this.Column == other.Column;
            }

            return false;
        }

        public override int GetHashCode()
        {
            // Customize the hash code generation based on your requirements
            // Ensure that objects that are considered equal return the same hash code
            // For example:
            int hash = 17;
            hash = hash * 23 + Row.GetHashCode();
            hash = hash * 23 + Column.GetHashCode();
            return hash;
        }

        //public RoadConfiguration Road_Config { get; set; }
    }
}
