using System.Diagnostics;

namespace NEATDrive_WPF.DrivingScripts
{
    internal class RoadManager
    {
        public static RoadManager? instance;

        public RoadConfiguration? roadConfiguration = new RoadConfiguration();
        public Road? road = new Road();

        /// <summary>
        /// Assigns the road configuration to the road (Applies road textures on Roads)
        /// </summary>
        public void AssignRoadConfiguration()
        {
            road.UpdateCell(0, roadConfiguration);
        }
        //public void InitializeRoadCanvas()
        //{
        //    ApplicationManager.instance?.simWindow.AddRoadBordersToList();
        // }

        public void RefreshRoadConfig()
        {
            if (ApplicationManager.instance?.configWindow.roadSlotList.Count > 0)
            {
                Debug.WriteLine(ApplicationManager.instance?.configWindow.roadSlotList.Count);
                Debug.WriteLine(ApplicationManager.instance?.configWindow.roadSlotList[0].SelectedImage);
            }

            for (int i = 0; i < ApplicationManager.instance?.configWindow.roadSlotList.Count; i++)
            {
                // Check if the RoadSlot is already in the RoadCanvas
                //Canvas roadCanvas = roadCanvases.FirstOrDefault(rc => rc.Name == $"Road{i + 1}");

                // Get the URI of the RoadSlot and create a new ImageBrush
                //ImageBrush imageBrush = new ImageBrush(ApplicationManager.instance.configWindow.roadSlotList[i].SelectedImage);

                // Set the ImageBrush as the background of the RoadCanvas
                //ApplicationManager.instance.simWindow.roadCanvasList[i].Background = imageBrush;

                Debug.WriteLine(ApplicationManager.instance.simWindow.roadCanvasList[i]);
            }
        }


    }





}
