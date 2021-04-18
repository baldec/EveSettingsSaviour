using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EveSettingsSaviour.Models.ESI
{
    public class Character
    {
        public int? Id { get; set; }

        [JsonPropertyName("alliance_id")]
        public int? AllianceId { get; set; }

        [JsonPropertyName("ancestry_id")]
        public int? AncestryId { get; set; }

        [JsonPropertyName("birthday")] 
        public DateTime? Birthday { get; set; }

        [JsonPropertyName("bloodline_id")] 
        public int? BloodlineId { get; set; }

        [JsonPropertyName("corporation_id")] 
        public int? CorporationId { get; set; }

        [JsonPropertyName("description")] 
        public string Description { get; set; }

        [JsonPropertyName("gender")] 
        public string Gender { get; set; }

        [JsonPropertyName("name")] 
        public string Name { get; set; }

        [JsonPropertyName("race_id")] 
        public int? RaceId { get; set; }

        [JsonPropertyName("security_status")] 
        public decimal SecurityStatus { get; set; }


    }
    //    alliance_id: 99008809,
    //ancestry_id: 7,
    //birthday: "2018-01-11T14:26:23Z",
    //bloodline_id: 2,
    //corporation_id: 98582473,
    //description: "",
    //gender: "male",
    //name: "Edurus AdSanandum",
    //race_id: 1,
    //security_status: 5.005169222,
}
