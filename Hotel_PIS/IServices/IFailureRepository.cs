using Hotel_PIS.DAL;
using Hotel_PIS.IServices.Shared;

namespace Hotel_PIS.IServices
{
    public interface IFailureRepository
    {
        public Failure Save(int id, Failure obj);
        public bool Delete(int id);

        public bool Solve(int id);
    }
}