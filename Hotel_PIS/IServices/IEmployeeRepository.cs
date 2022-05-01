using Hotel_PIS.DAL;
using Hotel_PIS.DAL.Dto;
using Hotel_PIS.IServices.Shared;

namespace Hotel_PIS.IServices
{
    public interface IEmployeeRepository
    {
        public Employee Save(int id, Employee obj);
        public bool Delete(int id);
        public Employee Get(int id);
        public List<Employee> GetAll();
        UserDto Login(UserDto user);

    }
}
