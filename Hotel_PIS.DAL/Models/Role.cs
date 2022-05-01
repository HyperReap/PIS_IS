using System.Text.Json.Serialization;

namespace Hotel_PIS.DAL
{
    public class Role : BaseModel
    {
        public Role()
        {
        }
        public string NameOfRole { get; set; }
        [JsonIgnore]
        public ICollection<Employee>? EmployeesWithRole { get; set; }
    }
}