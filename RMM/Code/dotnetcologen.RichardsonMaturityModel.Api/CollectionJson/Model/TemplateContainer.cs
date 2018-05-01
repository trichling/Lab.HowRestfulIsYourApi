using Gach.CollectionJson.Model.Common;

namespace Gach.CollectionJson.Model
{
    public abstract class TemplateContainer<TDataElement> : ValueObject<TemplateContainer<TDataElement>>
        where TDataElement : DataElement
    {
        public abstract Template<TDataElement> Template { get; }
    }
}
