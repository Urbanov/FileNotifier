using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace FileNotifier.Models
{
    class Watcher
    {
        private FileSystemWatcher watcher;

        public Watcher()
        {
            watcher = new FileSystemWatcher();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void watch(string path)
        {
            watcher.Path = path;
            watcher.EnableRaisingEvents = true;
            watcher.Created += new FileSystemEventHandler(FileSystemWatcher_FileCreated);
        }

        public event FileSystemEventHandler NewFileCreated;
        private void FileSystemWatcher_FileCreated(object sender, FileSystemEventArgs e)
        {
            if ((new FileInfo(e.FullPath).Attributes & FileAttributes.Hidden) == 0)
            {
                NewFileCreated?.Invoke(sender, e);
            }
        }
    }
}
