using System;
using System.Collections.Generic;

using Gach.CollectionJson.Model.Common;

namespace Gach.CollectionJson.Model
{
    public abstract class Item<TDataElement, TLink> : ValueObject<Item<TDataElement, TLink>>
        where TDataElement : DataElement
        where TLink : Link
    {
        public abstract Uri Href { get; set; }

        public abstract ICollection<TDataElement> Data { get; set; }

        public abstract ICollection<TLink> Links { get; set; }


    }
}