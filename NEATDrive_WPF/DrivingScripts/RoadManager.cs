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
        public RoadConfiguration Road_Config { get; set; } 
    }



}
