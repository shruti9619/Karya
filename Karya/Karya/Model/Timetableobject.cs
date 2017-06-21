using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Karya.Core
{
    public class Timetableobject
    {

        [SQLite.PrimaryKey]
        [JsonProperty(PropertyName = "Ttobjectid")]
        public string Ttobjectid { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "Userid")]
        public int Userid { get; set; }

        [JsonProperty(PropertyName = "day")]
        public string day { get; set; }

        [JsonProperty(PropertyName = "row")]
        public int row { get; set; }

        [JsonProperty(PropertyName = "col")]
        public int col { get; set; }

        [JsonProperty(PropertyName = "Subjectname")]
        public string Subjectname { get; set; }

        [JsonProperty(PropertyName = "Teachername")]
        public string Teachername { get; set; }

        [JsonProperty(PropertyName = "timestart")]
        public string timestart { get; set; }
    }
}
