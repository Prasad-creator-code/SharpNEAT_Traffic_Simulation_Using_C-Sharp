using NEATDrive_WPF.DrivingScripts.RoadSlots;
using System.Collections.Generic;

namespace NEATDrive_WPF.DrivingScripts
{
    public class RoadConfiguration
    {

    }

    // In your Road class, use a List<Cell> to represent the cells in the road grid
    public class Road
    {
        private List<RoadSlot> roadSlots; // List to store the cells in the road grid



        // Constructor to initialize the road grid with empty cells
        public Road()
        {
            //roadSlots = new List<RoadSlot>();
            for (int i = 0; i < 9; i++)
            {

                //roadSlots.Add(new RoadSlot());
            }
        }

        // Method to update the road configuration in a specific cell
        public void UpdateCell(int index, RoadConfiguration configuration)
        {
            // Update the configuration of the cell at the specified index in the list
            //roadSlots[index].Road_Config = configuration;
        }

        // Other methods or properties as needed for your specific requirements
    }




}
