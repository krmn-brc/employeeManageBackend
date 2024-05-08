using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities
{
    public class Doctor:OtherBaseEntity
    {
        
        [Required]    
        public DateTime Date { get; set; }
        
        [Required]
        public string? MedicalDiagnose { get; set; }

        [Required]        
        public string? MedicalRecommendation { get; set; }
    }
}