using System;
using System.Collections.Generic;

using Gach.CollectionJson.Model.Common;

namespace Gach.CollectionJson.Model
{
    public abstract class Query<TDataElement> : ValueObject<Query<TDataElement>>
        where TDataElement: DataElement
    {
        public abstract string Rel { get; }

        public abstract Uri Href { get; }

        public abstract string Name { get; set; }

        public abstract string Prompt { get; set; }

        public abstract ICollection<TDataElement> Data { get; set; }
    }
}