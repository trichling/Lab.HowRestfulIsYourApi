using Gach.CollectionJson.Model.Common;

namespace Gach.CollectionJson.Model
{
    public abstract class CollectionContainer<TCollection, TLink, TItem, TQuery, TTemplate, TError,TDataElement> 
        : ValueObject<CollectionContainer<TCollection, TLink, TItem, TQuery, TTemplate, TError, TDataElement>>
        where TCollection : Collection<TLink, TItem, TQuery, TTemplate, TError, TDataElement>
        where TDataElement: DataElement
        where TLink : Link
        where TItem : Item<TDataElement,TLink>
        where TQuery : Query<TDataElement>
        where TTemplate : Template<TDataElement>
        where TError : Error
    {
        public abstract TCollection Collection { get; }
    }
}