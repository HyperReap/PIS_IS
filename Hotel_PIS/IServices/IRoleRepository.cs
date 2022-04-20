using Hotel_PIS.DAL;
using Hotel_PIS.IServices.Shared;

namespace Hotel_PIS.IServices
{
    public interface IRoleRepository
    {
        public Role Save(int id, Role obj);
        public bool Delete(int id);
        public Role Get(int id);
        public List<Role> GetAll();
        public bool AssignRole(int id, Employee employee);
    }
}
