using EveSettingsSaviour.Models;
using System.Windows.Controls;

namespace EveSettingsSaviour.Controls
{
    /// <summary>
    /// Interaction logic for UserSettingsControl.xaml
    /// </summary>
    public partial class UserSettingsControl : UserControl
    {
        public UserFile UserFile { get; }

        public UserSettingsControl(UserFile userFile)
        {
            InitializeComponent();
            UserFile = userFile;

            cb_WillOverwrite.Content = $"{UserFile.Id} - {UserFile.LastEdited}";
        }
    }
}
