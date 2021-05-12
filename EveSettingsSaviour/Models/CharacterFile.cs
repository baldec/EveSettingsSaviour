using EveSettingsSaviour.ESI.Models;
using System;

namespace EveSettingsSaviour.Models
{
    public class CharacterFile
    {
        public int? Id { get; set; }

        public DateTime? LastEdited { get; set; }

        public Character Character { get; set; }

        public string FilePath { get; set; }
    }
}
