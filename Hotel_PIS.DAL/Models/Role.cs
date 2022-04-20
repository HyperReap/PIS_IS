namespace Hotel_PIS.DAL
{
    public class Role : BaseModel
    {
        public Role()
        {
        }
        public string NameOfRole { get; set; }
        public ICollection<Employee> EmployeesWithRole { get; set; }
    }
}