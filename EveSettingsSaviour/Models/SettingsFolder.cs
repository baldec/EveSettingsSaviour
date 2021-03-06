using System.Collections.Generic;
using System.IO;

namespace EveSettingsSaviour.Models
{
    public class SettingsFolder
    {
        public string FolderName { get; set; }
        public string FilePath { get; set; }

        public List<CharacterFile> CharacterFiles { get; set; }

        public List<UserFile> UserFiles { get; set; }

        public SettingsFolder(string filepath)
        {
            FilePath = filepath;
            CharacterFiles = new List<CharacterFile>();
            UserFiles = new List<UserFile>();
            FolderName = new DirectoryInfo(filepath).Name;
        }
    }
}
