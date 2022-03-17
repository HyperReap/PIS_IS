namespace Hotel_PIS.DAL
{
    public class Role : BaseModel
    {
        public Role()
        {
            Permissions = new List<Permission>();
        }
        public string NameOfRole { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}