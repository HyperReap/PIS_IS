using Hotel_PIS.DAL;
using Hotel_PIS.DAL.Dto;
using Hotel_PIS.IServices.Shared;

namespace Hotel_PIS.IServices
{
    public interface IFailureRepository
    {
        public FailureDto Save(Failure obj);
        public bool Delete(int id);
        public bool Solve(int failureId);
        public FailureDto Get(int id);
        public List<FailureDto> GetAll();
    }
}