using Hotel_PIS.DAL;
using Hotel_PIS.DAL.Dto;
using Hotel_PIS.IServices.Shared;

namespace Hotel_PIS.IServices
{
    public interface IReservationRepository
    {
        public Reservation Get(int id);
        public List<Reservation> GetAll();
        public void CancelReservation(int id);
        public void Pay(int id);
        public void PayDeposit(int id, Decimal amount);
        public void PayArrear(int id);
        public void CheckIn(int id);
        public void CheckOut(int id);
        Reservation Save(ReservationDto obj);
        List<FromToDateDto> GetBookedDatesOfRooms(List<int> roomIds, DateTime dateNow);
        List<Reservation> GetReservationsByEmail(string email);
    }
}
