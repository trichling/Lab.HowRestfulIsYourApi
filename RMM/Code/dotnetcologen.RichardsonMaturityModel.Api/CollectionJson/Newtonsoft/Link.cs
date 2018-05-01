using System;

using Newtonsoft.Json;

namespace Gach.CollectionJson.Model.Newtonsoft
{
    public class Link : Model.Link
    {
        [JsonProperty("rel", NullValueHandling = NullValueHandling.Ignore)]
        public override string Rel { get; }

        [JsonProperty("href", NullValueHandling = NullValueHandling.Ignore)]
        public override Uri Href { get; }

        [JsonProperty("prompt", NullValueHandling = NullValueHandling.Ignore)]
        public override string Prompt { get; set; }

        [JsonProperty("render", NullValueHandling = NullValueHandling.Ignore)]
        public override string Render { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public override string Name { get; set; }

        [JsonConstructor]
        public Link(string rel, Uri href)
        {
            Rel = rel;
            Href = href;
        }
    }
}