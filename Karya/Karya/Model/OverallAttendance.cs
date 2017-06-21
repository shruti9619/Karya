using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Karya.Core
{
    public class OverallAttendance
    {
        [SQLite.PrimaryKey]
        [JsonProperty(PropertyName = "OAAttendanceid")]
        public int OAAttendanceid { get; set; }

        [JsonProperty(PropertyName = "Userid")]
        public int Userid { get; set; }

        [JsonProperty(PropertyName = "id")]
        public String id { get; set; }

        [JsonProperty(PropertyName = "Totalclass")]
        public int Totalclass { get; set; }

        [JsonProperty(PropertyName = "Attendedclass")]
        public int Attendedclass { get; set; }

        [JsonProperty(PropertyName = "Bunkedclass")]
        public int Bunkedclass { get; set; }
    }

}
