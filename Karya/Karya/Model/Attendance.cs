using System;
using Newtonsoft.Json;

namespace Karya.Core
{
    public class Attendance
    {
        [SQLite.PrimaryKey]
        [JsonProperty(PropertyName = "Attendid")]
        public int Attendid { get; set; }

        [JsonProperty(PropertyName = "OAAttendanceid")]
        public int OAAttendanceid { get; set; }

        [JsonProperty(PropertyName = "Subjectname")]
        public string Subjectname { get; set; }

        [JsonProperty(PropertyName = "id")]
        public String id { get; set; }

        [JsonProperty(PropertyName = "Totalclass")]
        public int Totalclass { get; set; }

        [JsonProperty(PropertyName = "Attendedclass")]
        public int Attendedclass { get; set; }

        [JsonProperty(PropertyName = "Isattended")]
        public bool Isattended { get; set; }

        [JsonProperty(PropertyName = "Isbunked")]
        public bool Isbunked { get; set; }

        [JsonProperty(PropertyName = "Isnoclass")]
        public bool Isnoclass { get; set; }

        [JsonProperty(PropertyName = "Bunkedclass")]
        public int Bunkedclass { get; set; }

        [JsonProperty(PropertyName = "Timestart")]
        public TimeSpan timestart { get; set; }

        [JsonProperty(PropertyName = "Date")]
        public DateTime Date { get; set; }


        public Attendance()
        {
            this.Attendedclass = 0;
            this.Bunkedclass = 0;
            this.Totalclass = 0;
            this.Isattended = false;
            this.Isbunked = false;
            this.Isnoclass = true;
        }

        
    }
}
