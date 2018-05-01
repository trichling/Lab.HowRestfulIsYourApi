using Gach.CollectionJson.Model.Common;

namespace Gach.CollectionJson.Model
{
    public abstract class Error : ValueObject<Error>
    {
        public abstract string Title { get; set; }

        public abstract string Code { get; set; }

        public abstract string Message { get; set; }
    }
}