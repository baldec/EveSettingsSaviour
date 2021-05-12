using System;

namespace EveSettingsSaviour.Models
{
    public class UserFile
    {
        public int? Id { get; set; }

        public DateTime? LastEdited { get; set; }
        public string FilePath { get; set; }
    }
}
