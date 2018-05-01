using Newtonsoft.Json;

namespace Gach.CollectionJson.Model.Newtonsoft
{
    public class TemplateContainer : TemplateContainer<DataElement>
    {
        [JsonProperty("template")]
        public override Template<DataElement> Template { get; }

        public TemplateContainer(Template template)
        {
            Template = template;
        }
    }
}
