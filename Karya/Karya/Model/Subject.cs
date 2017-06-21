using System;
using Newtonsoft.Json;

namespace Karya.Core
{
    public class Subject
    {
        [SQLite.PrimaryKey]
        [JsonProperty(PropertyName = "Subjectid")]
        public int Subjectid { get; set; }

        [JsonProperty(PropertyName = "id")]
        public String id { get; set; }

        [JsonProperty(PropertyName = "Userid")]
        public int Userid { get; set; }

        [JsonProperty(PropertyName = "Subjectname")]
        public String Subjectname { get; set;}

        [JsonProperty(PropertyName = "Teacherid")]
        public int Teacherid { get; set; }

        [JsonProperty(PropertyName = "Teachername")]
        public String Teachername { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime createdAt { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public DateTime updatedAt { get; set; }

        [JsonProperty(PropertyName = "version")]
        public String version { get; set; }

    }
}
