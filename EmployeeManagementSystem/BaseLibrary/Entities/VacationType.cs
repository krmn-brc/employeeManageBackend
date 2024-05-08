using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    public class VacationType : BaseEntity
    {
        // Manyt to one relationship with Vacation
        [JsonIgnore]
        public ICollection<Vacation>? Vacations{ get; set; }
    }
}