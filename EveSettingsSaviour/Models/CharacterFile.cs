using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EveSettingsSaviour.Models.ESI;

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
