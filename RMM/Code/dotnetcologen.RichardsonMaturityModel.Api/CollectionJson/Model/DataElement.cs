using Gach.CollectionJson.Model.Common;

namespace Gach.CollectionJson.Model
{
    public abstract class DataElement : ValueObject<DataElement>
    {
        public abstract string Name { get; }

        public abstract string Value { get; set; }

        public abstract string Prompt { get; set; }

    }
}