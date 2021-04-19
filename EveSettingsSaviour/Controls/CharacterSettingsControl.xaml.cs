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
    /// Interaction logic for CharacterSettingsControl.xaml
    /// </summary>
    public partial class CharacterSettingsControl : UserControl
    {
        CharacterFile CharacterFile { get; }
        public CharacterSettingsControl(CharacterFile characterFile)
        {
            InitializeComponent();
            CharacterFile = characterFile;

            cb_WillOverwrite.Content = $"{CharacterFile.Character.Name}({CharacterFile.Id})";
        }
    }
}
