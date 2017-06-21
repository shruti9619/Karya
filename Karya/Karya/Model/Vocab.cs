using System;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace Karya.Core
{
    public class Vocab
    {
        public int Userid { get; set; }

        [SQLite.PrimaryKey]
        [JsonProperty(PropertyName = "Vocabid")]
        public int Vocabid { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "Word")]
        public string Word { get; set; }

        [JsonProperty(PropertyName = "Meaning")]
        public string Meaning { get; set; }

        [JsonProperty(PropertyName = "Synonym")]
        public string Synonym { get; set; }

        [JsonProperty(PropertyName = "Usage")]
        public string Usage { get; set; }

        [JsonProperty(PropertyName = "Example")]
        public string Example { get; set; }

        [JsonProperty(PropertyName = "Antonym")]
        public string Antonym { get; set; }

        [JsonProperty(PropertyName = "Category")]
        public string Category { get; set; }
    }
}
