using Newtonsoft.Json;

namespace Gach.CollectionJson.Model.Newtonsoft
{
    public class Error : Model.Error
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public override string Title { get; set; }
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public override string Code { get; set; }
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public override string Message { get; set; }
    }
}