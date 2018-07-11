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
            watcher.Created += new FileSystemEventHandler(FileSystemWatcher_FileCreated);
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void watch(string path)
        {
            watcher.Path = path;
            watcher.EnableRaisingEvents = true;
        }

        public event FileSystemEventHandler NewFileCreated;
        private void FileSystemWatcher_FileCreated(object sender, FileSystemEventArgs e)
        {
            FileInfo file = new FileInfo(e.FullPath);
            if ((file.Attributes & (FileAttributes.Hidden | FileAttributes.Temporary | FileAttributes.System)) == 0 && file.Extension != ".tmp")
            {
                NewFileCreated?.Invoke(sender, e);
            }
        }
    }
}
