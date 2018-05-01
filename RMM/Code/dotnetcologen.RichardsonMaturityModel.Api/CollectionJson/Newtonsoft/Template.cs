using System.Collections.Generic;

using Newtonsoft.Json;

namespace Gach.CollectionJson.Model.Newtonsoft
{
    public class Template : Template<DataElement>
    {
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public override ICollection<DataElement> Data { get; set; }

        [JsonConstructor]
        public Template()
        {
            Data = new List<DataElement>();
        }
       
       
    }
}