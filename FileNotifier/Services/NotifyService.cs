using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileNotifier.Services
{
    class NotifyService
    {
        private const int maxNotifications = 3;

        private Queue<string> messages;
        private bool inProgress;
        private readonly object sync;

        public NotifyIcon NotifyIcon
        {
            get;
            private set;
        }

        public NotifyService()
        {
            messages = new Queue<string>();
            inProgress = false;
            sync = new object();

            NotifyIcon = new NotifyIcon();
            NotifyIcon.Visible = true;
            NotifyIcon.Icon = Properties.Resources.icon;
            NotifyIcon.BalloonTipTitle = "New files";
            NotifyIcon.Text = "File Notifier";
            NotifyIcon.Click += NotifyIcon_Clicked;
            NotifyIcon.ContextMenu = new ContextMenu(new MenuItem[]
            {
                new MenuItem("Exit", ContextMenu_Clicked)
            });
        }

        public void Notify(string message)
        {
            lock (sync)
            {
                messages.Enqueue(message);

                if (!inProgress)
                {
                    inProgress = true;
                    Task.Delay(2000).ContinueWith(task => ShowNotification());
                }
            }
        }

        private void ShowNotification()
        {
            lock (sync)
            {
                NotifyIcon.BalloonTipText = StackMessages();
                NotifyIcon.ShowBalloonTip(2000);
                inProgress = false;
            }
        }

        private string StackMessages()
        {
            string result = "";

            int numberOfMessages = messages.Count;
            for (int i = 0; i < Math.Min(maxNotifications, numberOfMessages); ++i)
            {
                result += messages.Dequeue() + "\n";
            }

            if (messages.Count > 0)
            {
                result += string.Format("and {0} more", messages.Count);
                messages.Clear();
            }
            else
            {
                result.TrimEnd();
            }

            return result;
        }

        public event EventHandler NotifyIconClicked;
        private void NotifyIcon_Clicked(object sender, EventArgs e)
        {
            NotifyIconClicked?.Invoke(sender, e);
        }

        public event EventHandler ExitRequested;
        private void ContextMenu_Clicked(object sender, EventArgs e)
        {
            ExitRequested?.Invoke(sender, e);
        }
    }
}
