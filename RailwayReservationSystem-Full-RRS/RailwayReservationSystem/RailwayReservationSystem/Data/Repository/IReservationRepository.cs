using RailwayReservationSystem.Models;
using System.Collections.Generic;

namespace RailwayReservationSystem.Data.Repository
{
    public interface IReservationRepository
    {
        public Ticket AddReservation(ReservationDto reservation, int userId, List<Passenger> passengers);
        Reservation AddPayment(Reservation reservation);
        public Ticket GetTicket(int pnr);
        public Ticket CancelTicket(int pnr);
    }
}
