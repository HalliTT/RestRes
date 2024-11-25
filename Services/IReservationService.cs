using MongoDB.Bson;
using RestRes.Models;

namespace RestRes.Services
{
    public interface IReservationService
    {
        IEnumerable<Reservation> GetAllReservations();
        Reservation? GetReservationById(ObjectId id);

        void AddReservation(Reservation newReservation);

        void EditReservation(Reservation updatedReservation);

        void DeleteReservation(Reservation reservationToDelete);
    }
}