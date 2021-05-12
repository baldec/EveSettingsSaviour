using EveSettingsSaviour.Models;
using System.Windows.Controls;

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
            lbl_FolderName.Content = settingsFolder.FolderName;
            txtBlock_FolderPath.Text = settingsFolder.FilePath;
        }

        private void InitializeCharacterFiles()
        {
            foreach (var s in SettingsFolder.CharacterFiles)
            {
                CharacterSettingsControl characterSetting = new CharacterSettingsControl(s);
                sp_characterFiles.Children.Add(characterSetting);
            }
        }

        private void InitializeUserFiles()
        {
            foreach (var s in SettingsFolder.UserFiles)
            {
                UserSettingsControl userSetting = new UserSettingsControl(s);
                sp_userFiles.Children.Add(userSetting);
            }
        }
    }
}
