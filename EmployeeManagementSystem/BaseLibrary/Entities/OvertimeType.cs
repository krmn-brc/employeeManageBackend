using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    public class OvertimeType : BaseEntity
    {
        [JsonIgnore]
        public ICollection<Overtime>? Overtimes{ get; set; }
    }
}