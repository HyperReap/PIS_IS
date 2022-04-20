using Hotel_PIS.DAL;
using Hotel_PIS.IServices.Shared;

namespace Hotel_PIS.IServices
{
    public interface IReservationRepository
    {
        public bool Delete(int id);
        public Reservation Get(int id);
        public List<Reservation> GetAll();
        public void CancelReservation(int id);
        public void Pay(int id);
        public void PayDeposit(int id, Decimal amount);
        public void PayArrear(int id);
        public void CheckIn(int id);
        public void CheckOut(int id);
        Reservation Save(Reservation obj, int roomId, DateTime dateTo, DateTime dateFrom);
    }
}
