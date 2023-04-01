using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NEATDrive_WPF
{
    /// <summary>
    /// THis is the master class of the entire application. Windows are meant to be called using
    /// these instance below not directly for the sake of clean code
    /// </summary>
    public class ApplicationManager
    {
        public static ApplicationManager? instance;

        public ConfigurationWindow configWindow = new();

        public void FocusCanvas(Canvas canvasToFocus)
        {
            canvasToFocus.Focus();
        }

    }
}
