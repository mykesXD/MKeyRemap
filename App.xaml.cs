using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Forms = System.Windows.Forms;
namespace KeyRemap
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Forms.NotifyIcon _notifyIcon;

        public App()
        {
            _notifyIcon = new Forms.NotifyIcon();
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            _notifyIcon.Icon = new System.Drawing.Icon("image/dm.ico");
            _notifyIcon.Text = "Desktop Maid";
            _notifyIcon.Visible = true;
            _notifyIcon.ContextMenu = new Forms.ContextMenu();
            _notifyIcon.ContextMenu.MenuItems.Add("Open", new EventHandler(Open));
            _notifyIcon.ContextMenu.MenuItems.Add("Exit", new EventHandler(Quit));

            base.OnStartup(e);
        }
        private void Open(object sender, EventArgs e)
        {
            MainWindow.Visibility = Visibility.Visible;
        }
        private void Quit(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            Environment.Exit(0);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            base.OnExit(e);
        }
    }
}