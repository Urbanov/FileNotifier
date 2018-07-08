using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace FileNotifier.Services
{
    class WindowService
    {
        private MainWindow window;

        public WindowService(MainWindow window)
        {
            this.window = window;
            this.window.Closing += MainWindow_Closing;
            this.window.StateChanged += MainWindow_StateChanged;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            window.Hide();
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (window.WindowState == WindowState.Minimized)
            {
                window.Hide();
            }
        }

        public void OpenSettingsWindow()
        {
            window.Show();
            window.WindowState = WindowState.Normal;
        }

        public string OpenBrowseDialog()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            return result == DialogResult.OK ? dialog.SelectedPath : null;
        }

        public void Close()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
