using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.DTOs
{
    public class EmployeeGroup1
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? TelephoneNumber { get; set; }

        [Required]
        public string? Photo { get; set; }

        [Required]
        public string? CivilId { get; set; }

        [Required]
        public string? FileNumber { get; set; }
    }
}