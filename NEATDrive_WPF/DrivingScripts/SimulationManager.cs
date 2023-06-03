using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;

namespace NEATDrive_WPF.DrivingScripts
{
    /// <summary>
    /// This class contains the simulation parameters to be used during the sim and the NEAT working scripts
    /// </summary>
    class SimulationManager
    {
        public static SimulationManager? instance;

        DataGrid myDataGrid;
        List<MyData> dataList = new()
        {
            new MyData() { ID = 1, CrashTime = 25, Crossings = 4, PedInteractions=2, MaxGenerations=53, Crashes=4},
            new MyData() { ID = 2, CrashTime = 30, Crossings = 2, PedInteractions=3, MaxGenerations=68, Crashes=2},
            new MyData() { ID = 3, CrashTime = 35, Crossings = 6, PedInteractions=7, MaxGenerations=34, Crashes=1 }
        };

        public class MyData
        {
            public int ID { get; set; }
            public int CrashTime { get; set; }
            public int Crossings { get; set; }
            public int PedInteractions { get; set; }
            public int MaxGenerations { get; set; }
            public int Crashes { get; set; }
        }


        public void FillData(DataGrid dataGrid)
        {
            myDataGrid = dataGrid;

            // Define the columns
            myDataGrid.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("ID") });
            myDataGrid.Columns.Add(new DataGridTextColumn { Header = "Avg Crash Time (in mins)", Binding = new Binding("CrashTime") });
            myDataGrid.Columns.Add(new DataGridTextColumn { Header = "Crossings", Binding = new Binding("Crossings") });
            myDataGrid.Columns.Add(new DataGridTextColumn { Header = "Ped Interactions", Binding = new Binding("PedInteractions") });
            myDataGrid.Columns.Add(new DataGridTextColumn { Header = "Max Generations", Binding = new Binding("MaxGenerations") });
            myDataGrid.Columns.Add(new DataGridTextColumn { Header = "Crashes", Binding = new Binding("Crashes") });

            // Add data to the DataGrid
            myDataGrid.ItemsSource = dataList;
        }







    }
}

