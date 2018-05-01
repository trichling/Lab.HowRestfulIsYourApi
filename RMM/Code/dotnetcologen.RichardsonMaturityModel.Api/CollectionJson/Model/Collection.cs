using System;
using System.Collections.Generic;

using Gach.CollectionJson.Model.Common;

namespace Gach.CollectionJson.Model
{
    public abstract class Collection<TLink,TItem,TQuery,TTemplate,TError,TDataElement> : ValueObject<Collection<TLink, TItem, TQuery, TTemplate, TError, TDataElement>>
        where TDataElement :DataElement
        where TLink : Link
        where TItem:Item<TDataElement,TLink>
        where TQuery:Query<TDataElement>
        where TTemplate : Template<TDataElement>
        where TError : Error
    {
        public const string MediaType = "application/vnd.collection+json";
        public abstract string Version { get; }

        public abstract Uri Href { get; }

        public abstract ICollection<TLink> Links { get; set; }

        public abstract ICollection<TItem> Items { get; set; }

        public abstract ICollection<TQuery> Queries { get; set; }

        public abstract TTemplate Template { get; set; }

        public abstract TError Error { get; set; }

    }
}