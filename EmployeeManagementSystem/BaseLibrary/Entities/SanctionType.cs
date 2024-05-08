using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    public class SanctionType : BaseEntity
    {
        [JsonIgnore]
        public ICollection<Sanction>? Sanctions{ get; set; }
    }
}