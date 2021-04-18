using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveSettingsSaviour.Models
{
    public class UserFile
    {
        public int? Id { get; set; }

        public DateTime? LastEdited { get; set; }
        public string FilePath { get; set; }
    }
}
