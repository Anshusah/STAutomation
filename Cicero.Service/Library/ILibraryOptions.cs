using Newtonsoft.Json;

namespace Cicero.Service.Library
{
    public interface ILibraryOptions
    {
        [JsonIgnore]
        string Json { get; }
    }
}