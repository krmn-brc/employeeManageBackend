using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities
{
    public class Sanction : OtherBaseEntity
    {

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string? Punishment { get; set; }

        [Required]        
        public DateTime PunishmentDate { get; set; }

        // Many to one relationship with Sanction Type

        public SanctionType? SanctionType { get; set; }
        public int SanctionTypeId { get; set; }
    }
}