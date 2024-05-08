using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    public class City : BaseEntity
    {
        // Many to one relationship with Country
        public int CountryId { get; set; }
        public Country? Country { get; set; }

        // One to many relations with Town 
        [JsonIgnore]
        public ICollection<Town>? Towns { get; set; }
    }
}