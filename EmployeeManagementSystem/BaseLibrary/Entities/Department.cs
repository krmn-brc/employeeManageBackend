using System.Text.Json.Serialization;

namespace BaseLibrary.Entities;

public class Department : BaseEntity
{
    // Many to one relationship with General  Department
    public int GeneralDepartmentId { get; set; }
    public GeneralDepartment? GeneralDepartment { get; set; }

    // One to many relationship with Branch
    [JsonIgnore]
    public ICollection<Branch>? Branches { get; set; }
}
