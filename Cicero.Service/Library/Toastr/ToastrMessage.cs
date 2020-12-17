using Cicero.Service.Helpers;
using Newtonsoft.Json;

namespace Cicero.Service.Library.Toastr
{
    public class ToastrMessage : IToastMessage
    {
        public ToastrMessage(string message, ILibraryOptions options = null)
        {
            Message = message;
            Options = options;
        }
        [JsonProperty]
        public string Message { get; private set; }
        [JsonProperty]
        [JsonConverter(typeof(ConcreteTypeConverter<ToastrOptions>))]
        public ILibraryOptions Options { get; private set; }

    }
}
