using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Gach.CollectionJson.Model.Newtonsoft
{
    public class Item : Item<DataElement,Link>
    {
        [JsonProperty("href", NullValueHandling = NullValueHandling.Ignore)]
        public override sealed Uri Href { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public override sealed ICollection<DataElement> Data { get; set; }

        [JsonProperty("links", NullValueHandling = NullValueHandling.Ignore)]
        public override sealed ICollection<Link> Links { get; set; }

        public Item(Uri itemHrefUri)
        {
            Href = itemHrefUri;
        }
    }
}