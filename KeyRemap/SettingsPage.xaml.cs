using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            MainPage.mainPageInstance.selectedRowIndex = -1;
            this.NavigationService.GoBack();

        }

        private void StartupCheckbox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(StartupCheck.Visibility == Visibility.Visible)
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
        }
    }
}
