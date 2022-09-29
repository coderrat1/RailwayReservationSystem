using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RailwayReservationSystem.Models;
using System;
using System.Linq;

namespace RailwayReservationSystem.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private RailwayDbContext _context;
        private readonly ILogger<UserRepository> _log;

        public UserRepository(RailwayDbContext context, ILogger<UserRepository> logger) //Implementing Dependency Injection
        {
            _context = context;
            _log = logger;
        }

        #region "Check User"
        public User CheckUser(Login login)
        {
            try
            {
                if (login != null)
                    return _context.Users.FirstOrDefault(u => u.UserId == login.UserId && u.Password == login.Password);
            }
            catch (Exception exc)
            {
                _log.LogError(exc.Message);
            }
            return null;

        }
        #endregion

        #region "Check Email"
        public bool CheckEmail(Register reg)
        {
            try
            {
                if (reg != null)
                    return _context.Users.FirstOrDefault(u => u.Email == reg.Email) == null;
            }
            catch(Exception exc)
            {
                _log.LogError(exc.Message);
            }
            return false;

        }
        #endregion

        #region Add User"
        public User AddUser(Register reg)
        {
            User user = new User
            {
                Name = reg.Name,
                Email = reg.Email,
                Dob = reg.Dob,
                Password = reg.Password,
                Gender = reg.Gender,
                Role = "User"
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
        #endregion
    }
}
