using System.Collections.Generic;

using Gach.CollectionJson.Model.Common;

namespace Gach.CollectionJson.Model
{
    public abstract class Template <TDataElement> : ValueObject<Template<TDataElement>>
        where TDataElement:DataElement
    {
        public abstract ICollection<TDataElement> Data { get; set; } 
    }
}