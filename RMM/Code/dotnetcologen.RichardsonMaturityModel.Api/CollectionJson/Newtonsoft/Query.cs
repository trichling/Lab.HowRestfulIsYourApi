using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gach.CollectionJson.Model.Newtonsoft
{
    public  class Query : Query<DataElement>
    {
        [JsonProperty("rel", NullValueHandling = NullValueHandling.Ignore)]
        public override string Rel { get; }

        [JsonProperty("href", NullValueHandling = NullValueHandling.Ignore)]
        public override Uri Href { get; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public override string Name { get; set; }

        [JsonProperty("prompt", NullValueHandling = NullValueHandling.Ignore)]
        public override string Prompt { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public override ICollection<DataElement> Data { get; set; }

        public Query(string rel, Uri href)
        {
            Rel = rel;
            Href = href;
        }


    }
}