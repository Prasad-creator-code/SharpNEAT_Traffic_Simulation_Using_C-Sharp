using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace NEATDrive_WPF
{
    /// <summary>
    /// Interaction logic for SimulationWindow.xaml
    /// </summary>
    public partial class SimulationWindow : Window
    {
        public SimulationWindow()
        {
            InitializeComponent();
            AddRoadBordersToList();

        }
        public List<Canvas> roadCanvasList = new List<Canvas>();

        public void AddRoadBordersToList()
        {
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    string canvasName = "Road" + i.ToString() + "_" + j.ToString();
                    Canvas canvas = (Canvas)FindName(canvasName);

                    if (canvas != null && !roadCanvasList.Contains(canvas))
                    {
                        roadCanvasList.Add(canvas);
                    }
                }
            }
        }


        private void OnWindowClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //ApplicationManager.instance?.simWindow.Hide();
            this.Hide();
            ApplicationManager.instance?.configWindow.Show();
            //ApplicationManager.instance?.configWindow.Focus();
        }

        private void Start_Sim_Text_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
