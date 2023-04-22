using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEATDrive_WPF.DrivingScripts
{
    public class RoadConfiguration
    {

    }

    // In your Road class, use a List<Cell> to represent the cells in the road grid
    public class Road
    {
        private List<Cell> cells; // List to store the cells in the road grid

        // Constructor to initialize the road grid with empty cells
        public Road()
        {
            cells = new List<Cell>();
            for (int i = 0; i < 9; i++)
            {
                cells.Add(new Cell());
            }
        }

        // Method to update the road configuration in a specific cell
        public void UpdateCell(int index, RoadConfiguration configuration)
        {
            // Update the configuration of the cell at the specified index in the list
            cells[index].Road_Config = configuration;
        }

        // Other methods or properties as needed for your specific requirements
    }




}
