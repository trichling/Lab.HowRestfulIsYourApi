using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Gach.CollectionJson.Model.Newtonsoft
{
    public  class Collection : Collection<Link,Item,Query,Template,Error,DataElement>
    {
        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public override string Version { get; }

        [JsonProperty("href", NullValueHandling = NullValueHandling.Ignore)]
        public override Uri Href { get; }

        [JsonProperty("links", NullValueHandling = NullValueHandling.Ignore)]
        public override sealed ICollection<Link> Links { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public override ICollection<Item> Items { get; set; }

        [JsonProperty("queries", NullValueHandling = NullValueHandling.Ignore)]
        public override ICollection<Query> Queries { get; set; }

        [JsonProperty("template", NullValueHandling = NullValueHandling.Ignore)]
        public override Template Template { get; set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public override Error Error { get; set; }

        public Collection(Uri href)
        {
            Version = "1.0";
            Href = href;
        }
    }
}