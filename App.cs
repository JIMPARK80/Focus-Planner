using System;
using System.Windows;

namespace FocusPlanner
{
    public class App : Application
    {
        [STAThread]
        public static void Main()
        {
            var app = new App();
            var mainWindow = new MainWindow();
            mainWindow.Show();
            app.Run();
        }
    }
}

