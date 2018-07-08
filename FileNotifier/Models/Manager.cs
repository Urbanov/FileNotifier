using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileNotifier.Models
{
    class Manager
    {
        private const string registryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string appName = "FileNotifier";
        private const string propertyName = "Directory";

        public static void EnableAutorun()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath, true);
            key.SetValue(appName, Application.ExecutablePath);
        }

        public static void DisableAutorun()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath, true);
            key.DeleteValue(appName, false);
        }

        public static bool AutorunEnabled()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath, true);
            return key.GetValue(appName) != null;
        }

        public static string Directory
        {
            get
            {
                return (string)Properties.Settings.Default[propertyName];
            }
            set
            {
                Properties.Settings.Default[propertyName] = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
