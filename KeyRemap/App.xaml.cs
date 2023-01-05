using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
            _notifyIcon.Icon = new System.Drawing.Icon("image/remapIcon24x24.ico");
            _notifyIcon.Text = "MKeyRemap";
            _notifyIcon.Visible = true;
            _notifyIcon.ContextMenu = new Forms.ContextMenu();
            _notifyIcon.ContextMenu.MenuItems.Add("Open", new EventHandler(Open));
            _notifyIcon.ContextMenu.MenuItems.Add("Pause", new EventHandler(Pause));
            _notifyIcon.ContextMenu.MenuItems.Add("Exit", new EventHandler(Quit));

            base.OnStartup(e);
        }
        private void Open(object sender, EventArgs e)
        {
            MainWindow.ShowInTaskbar = true;
            MainWindow.Show();
            MainWindow.WindowState = WindowState.Normal;
            MainWindow.Activate();
            MainWindow.Topmost = true;
            MainWindow.Topmost = false;
            MainWindow.Focus();
        }
        private void Pause(object sender, EventArgs e)
        {
            MainPage.mainPageInstance.bind.BinderPause = false;
            MainPage.mainPageInstance.bind.BinderPause = true;
            if (MainPage.mainPageInstance.paused)
            {
                _notifyIcon.ContextMenu.MenuItems[1].Text = "Paused";
                MainPage.mainPageInstance.keyboardHookManager.Start();
                MainPage.mainPageInstance.paused = false;
                MainPage.mainPageInstance.PauseButton.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/image/pause.png")));
            }
            else
            {
                _notifyIcon.ContextMenu.MenuItems[1].Text = "Resume";
                MainPage.mainPageInstance.keyboardHookManager.Stop();
                MainPage.mainPageInstance.paused = true;
                MainPage.mainPageInstance.PauseButton.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/image/play.png")));
            }
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