using FileNotifier.Models;
using FileNotifier.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace FileNotifier.ViewModels
{
    class FileNotifierViewModel
    {
        private NotifyService notifyService;
        private WindowService windowService;
        private Watcher watcher;

        public FileNotifierViewModel(MainWindow window)
        {
            notifyService = new NotifyService();
            windowService = new WindowService(window);
            watcher = new Watcher();

            watcher.NewFileCreated += Notify;
            notifyService.NotifyIconClicked += OpenSettingsWindow;
            notifyService.ExitRequested += CloseApplication;

            if (!string.IsNullOrEmpty(Manager.Directory))
            {
                watcher.watch(Manager.Directory);
            }
        }

        public void Notify(object sender, FileSystemEventArgs e)
        {
            notifyService.Notify(e.Name);
        }

        public void OpenSettingsWindow(object sender, EventArgs e)
        {
            windowService.OpenSettingsWindow();
        }

        public void CloseApplication(object sender, EventArgs e)
        {
            windowService.Close();
        }

        public bool AutorunEnabled
        {
            get
            {
                return Manager.AutorunEnabled();
            }
        }

        public ICommand ToggleAutorun
        {
            get
            {
                return new RelayCommand(dummy =>
                {
                    if (AutorunEnabled)
                    {
                        Manager.DisableAutorun();
                    }
                    else
                    {
                        Manager.EnableAutorun();
                    }
                });
            }
        }

        public ICommand Browse
        {
            get
            {
                return new RelayCommand(dummy =>
                {
                    string dir = windowService.OpenBrowseDialog();
                    if (dir != null && !dir.Equals(Manager.Directory))
                    {
                        Manager.Directory = dir;
                        watcher.watch(Manager.Directory);
                    }
                });
            }
        }
    }
}
