using Hotel_PIS.DAL;
using Hotel_PIS.IServices.Shared;

namespace Hotel_PIS.IServices
{
    public interface IClientRepository/* : ICRUDRepository<Client>*/
    {
        public Client Save(int id, Client obj);
        public bool Delete(int id);
        public Client Get(int id);
        public List<Client> GetAll();
    }
}