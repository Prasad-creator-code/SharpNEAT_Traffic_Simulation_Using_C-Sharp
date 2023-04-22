using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEATDrive_WPF.DrivingScripts
{
    internal class RoadManager
    {
        public static RoadManager? instance;
    }

    // Define the Cell class to represent each cell in the road grid
    public class Cell
    {
        public RoadConfiguration Road_Config { get; set; } // Property to store the road configuration for this cell
                                                             // Add other properties or methods as needed for your specific requirements
    }



}
