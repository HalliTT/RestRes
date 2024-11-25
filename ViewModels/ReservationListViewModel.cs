using RestRes.Models;

namespace RestRes.ViewModels
{
    public class ReservationListViewModel
    {
        //public IEnumerable<Reservation>? Reservations { get; set; }
        public IEnumerable<IGrouping<dynamic, Reservation>> GroupedReservations { get; set; }
    }
}