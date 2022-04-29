using Hotel_PIS.DAL;
using Hotel_PIS.IServices.Shared;

namespace Hotel_PIS.IServices
{
    public interface IFailureRepository
    {
        public Failure Save(Failure obj);
        public bool Delete(int id);
        public bool Solve(int failureId);
        public Failure Get(int id);
        public List<Failure> GetAll();
    }
}