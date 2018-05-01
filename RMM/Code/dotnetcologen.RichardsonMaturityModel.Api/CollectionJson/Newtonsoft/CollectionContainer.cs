using Newtonsoft.Json;

namespace Gach.CollectionJson.Model.Newtonsoft
{
    public class CollectionContainer : CollectionContainer<Collection,Link, Item, Query, Template, Error, DataElement>
    {
        [JsonProperty("collection")]
        public override Collection Collection { get; }

        public CollectionContainer(Collection collection)
        {
            Collection = collection;
        }
    }
}