using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EveSettingsSaviour.Helpers;
using EveSettingsSaviour.Models;

namespace EveSettingsSaviour
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<SettingsFolder> _settingsFolders;

        public MainWindow()
        {
            InitializeComponent();

            
        }


        private  void btn_ScanSettings_Click(object sender, RoutedEventArgs e)
        {
            _settingsFolders = SettingsManager.ScanSettings(Enumerations.Servers.Tranquility).ToList();


            foreach (SettingsFolder s in _settingsFolders)
            {
                foreach (UserFile u in s.UserFiles)
                {

                }

                foreach (CharacterFile c in s.CharacterFiles)
                {
                    ComboBoxItem item = new ComboBoxItem();


                    item.Content = $"{s.FolderName} - {c.Character.Name}";

                    source_characterFile.Items.Add(item);
                }

            }
        }

        private async void btn_CopySettings_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
