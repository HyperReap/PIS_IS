using Hotel_PIS.DAL;
using Hotel_PIS.IServices.Shared;

namespace Hotel_PIS.IServices
{
    public interface IReservationRepository
    {
        public Reservation Save(int id, Reservation obj);
        public bool Delete(int id);
        public Reservation Get(int id);
        public List<Reservation> GetAll();
        public Reservation CreateReservation(Reservation obj);
        public Reservation UpdateReservation(Reservation obj);
        public void CancelReservation(int id);
        public void Pay(int id);
        public void PayDeposit(int id, Decimal amount);
        public void PayArrear(int id);
        public void CheckIn(int id);
        public void CheckOut(int id);
    }
}
