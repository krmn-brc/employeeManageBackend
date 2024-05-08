using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    public class Country : BaseEntity
    {
        [JsonIgnore]
        public ICollection<City>? Cities { get; set; }
    }
}