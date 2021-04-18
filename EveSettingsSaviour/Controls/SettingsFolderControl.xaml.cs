using EveSettingsSaviour.Models;
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

namespace EveSettingsSaviour.Controls
{
    /// <summary>
    /// Interaction logic for SettingsFolderControl.xaml
    /// </summary>
    public partial class SettingsFolderControl : UserControl
    {
        public SettingsFolder SettingsFolder { get; }

        public SettingsFolderControl(SettingsFolder settingsFolder)
        {
            SettingsFolder = settingsFolder;

            InitializeComponent();

            InitializeUserFiles();

            InitializeCharacterFiles();
        }

        private void InitializeCharacterFiles()
        {
            foreach (var s in SettingsFolder.CharacterFiles)
            {
                CharacterSettingsControl userSetting = new CharacterSettingsControl(s);
                sp_userFiles.Children.Add(userSetting);
            }
        }

        private void InitializeUserFiles()
        {
            foreach(var s in SettingsFolder.UserFiles)
            {
                UserSettingsControl userSetting = new UserSettingsControl(s);
                sp_userFiles.Children.Add(userSetting);
            }
        }
    }
}
