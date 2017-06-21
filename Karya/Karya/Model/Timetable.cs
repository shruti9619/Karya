using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Karya.Core
{
    public class Timetable
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "Userid")]
        public int Userid { get; set; }

        [SQLite.PrimaryKey]
        [JsonProperty(PropertyName = "Timetableid")]
        public int Timetableid{ get; set; }

        [JsonProperty(PropertyName = "Subjectid")]
        public int Subjectid { get; set; }


        [JsonProperty(PropertyName = "Lengthoftimeslot")]
        public int Lengthoftimeslot { get; set; }


        [JsonProperty(PropertyName = "Numofslot")]
        public int Numofslot { get; set; }


        [JsonProperty(PropertyName = "Timetableactive")]
        public Boolean Timetableactive { get; set; }


        [JsonProperty(PropertyName = "Schedulestring")]
        public string Schedulestring { get; set; }
    }
}
