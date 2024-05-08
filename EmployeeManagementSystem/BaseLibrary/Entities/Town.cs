using System.Text.Json.Serialization;

namespace BaseLibrary.Entities;

public class Town : BaseEntity
{
    
    // Many to one relationship with City
    public int CityId { get; set; }
    public City? City { get; set; }

    //Relationship: One to many with Employee
    [JsonIgnore]
    public ICollection<Employee>? Employees { get; set; }
}
