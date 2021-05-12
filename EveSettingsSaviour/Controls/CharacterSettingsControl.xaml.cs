using EveSettingsSaviour.Models;
using System.Windows.Controls;


namespace EveSettingsSaviour.Controls
{
    /// <summary>
    /// Interaction logic for CharacterSettingsControl.xaml
    /// </summary>
    public partial class CharacterSettingsControl : UserControl
    {
        public CharacterFile CharacterFile { get; }
        public CharacterSettingsControl(CharacterFile characterFile)
        {
            InitializeComponent();
            CharacterFile = characterFile;

            cb_WillOverwrite.Content = $"{CharacterFile.Character.Name}({CharacterFile.Id}) - {CharacterFile.LastEdited}";

        }
    }
}
