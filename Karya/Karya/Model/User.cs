using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Karya.Core
{
   public class User
    {
        [JsonProperty(PropertyName = "Userid")]
        public int Userid { get; set; }

        [JsonProperty(PropertyName = "id")]
        public String id { get; set; }

        [JsonProperty(PropertyName = "Username")]
        public String Username { get; set; }

        [JsonProperty(PropertyName = "Email")]
        public String Email { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime createdAt { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public DateTime updatedAt { get; set; }

        [JsonProperty(PropertyName = "version")]
        public String version { get; set; }
    }

}
