using System;

namespace NEATDrive_WPF.DrivingScripts.RoadSlots
{
    class RoadOptionSlot
    {
        public Uri? BitmapUri { get; set; }

        public RoadOptionSlot(Uri uri)
        {
            BitmapUri = uri;
        }
    }


}
