using System;

using Gach.CollectionJson.Model.Common;

namespace Gach.CollectionJson.Model
{
    public abstract class Link : ValueObject<Link>
    {
        public abstract string Rel { get; }

        public abstract  Uri Href { get; }

        public abstract string Prompt { get; set; }

        public abstract string Render { get; set; }

        public abstract string Name { get; set; }
    }
}