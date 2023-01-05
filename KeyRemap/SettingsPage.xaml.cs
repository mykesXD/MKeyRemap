using IWshRuntimeLibrary;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KeyRemap
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public static SettingsPage settingsPageInstance;
        public bool binderDelay;
        Bind bind;
        public SettingsPage()
        {
            InitializeComponent();
            DelaySlider.Value = MainPage.mainPageInstance.delay;
            bind = new Bind();
            bind.BinderDelay = false;
            DataContext = bind;
            settingsPageInstance = this;
        }

        private void ApplyButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (StartupCheck.Visibility == Visibility.Visible)
            {
                WshShell shell = new WshShell();
                string shortcutAddress = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\MKeyRemap.lnk";
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                shortcut.Description = "Shortcut for MKeyRemap";
                shortcut.IconLocation = Environment.CurrentDirectory + @"\MKeyRemap.exe, 0";
                shortcut.TargetPath = Environment.CurrentDirectory + @"\MKeyRemap.exe";
                shortcut.WorkingDirectory = Environment.CurrentDirectory;
                shortcut.Save();
            }
            else
            {
                System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\MKeyRemap.lnk");
            }

            MainPage.mainPageInstance.delay = (int)DelaySlider.Value;
            Save save = new Save(MainPage.mainPageInstance.delay, MainPage.mainPageInstance.keyMapList);
            var keyMapJson = JsonConvert.SerializeObject(save, Formatting.Indented);
            var path = Environment.CurrentDirectory;
            string filePath = System.IO.Path.Combine(path, "SaveFile.json");
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(keyMapJson);
            }
            MainPage.mainPageInstance.selectedRowIndex = -1;
            this.NavigationService.GoBack();
        }

        private void CancelButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MainPage.mainPageInstance.selectedRowIndex != -1)
            {
                var pain = Util.ChildrenInRow(MainPage.mainPageInstance.BodyContainer, MainPage.mainPageInstance.selectedRowIndex);
                Rectangle painer = (Rectangle)pain.ToList()[0];
                Rectangle painer2 = (Rectangle)pain.ToList()[2];
                painer2.StrokeThickness = 1;
                painer2.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF292C31");
                painer.Fill = Brushes.Transparent;
                MainPage.mainPageInstance.editPageOpen = false;
            }
            MainPage.mainPageInstance.selectedRowIndex = -1;
            this.NavigationService.GoBack();

        }

        private void StartupCheckbox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            if (StartupCheck.Visibility == Visibility.Visible)
            {
                StartupCheck.Visibility = Visibility.Hidden;
                bind.BinderDelay = false;
            }
            else
            {
                StartupCheck.Visibility = Visibility.Visible;
                bind.BinderDelay = true;
            }
        }

        private void SettingsMenu_Loaded(object sender, RoutedEventArgs e)
        {
            bind.BinderSettingsOpened = true;
            if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\MKeyRemap.lnk"))
            {
                StartupCheck.Visibility = Visibility.Visible;
                bind.BinderDelay = true;
            }
            else
            {
                StartupCheck.Visibility = Visibility.Hidden;
                bind.BinderDelay = false;
            }
        }

        private void Handle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindowInstance.DragMove();
        }
    }
}
