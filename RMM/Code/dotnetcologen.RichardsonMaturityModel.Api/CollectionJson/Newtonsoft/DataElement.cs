
using Newtonsoft.Json;

namespace Gach.CollectionJson.Model.Newtonsoft
{
    public class DataElement : Model.DataElement
    {
       [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public override string Name { get; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public override string Value { get; set; }

        [JsonProperty("prompt", NullValueHandling = NullValueHandling.Ignore)]
        public override string Prompt { get; set; }

        public DataElement(string name)
        {
            Name = name;
        }

    }
}