using RailwayReservationSystem.Models;

namespace RailwayReservationSystem.Data.Repository
{
    public interface IUserRepository
    {
        User CheckUser(Login login);
        bool CheckEmail(Register reg);
        User AddUser(Register reg);
    }
}
