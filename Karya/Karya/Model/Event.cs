using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Karya.Core
{
   public class Event
    {
        [SQLite.PrimaryKey]
        [JsonProperty(PropertyName = "Eventid")]
        public int Eventid { get; set; }

        [JsonProperty(PropertyName ="Userid")]
        public int Userid { get; set; }

        [JsonProperty(PropertyName = "Subjectid")]
        public int Subjectid { get; set; }

        [JsonProperty(PropertyName = "id")]
        public String id { get; set; }

        [JsonProperty(PropertyName = "Timetableid")]
        public int Timetableid { get; set; }

        [JsonProperty(PropertyName = "Title")]
        public String Title { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "Time")]
        public String Time { get; set; }

        [JsonProperty(PropertyName = "Date")]
        public String Date { get; set; }

        [JsonProperty(PropertyName = "Repeat")]
        public String Repeat { get; set; }

        [JsonProperty(PropertyName = "Datetime")]
        public DateTime Datetime { get; set; }




    }
}
