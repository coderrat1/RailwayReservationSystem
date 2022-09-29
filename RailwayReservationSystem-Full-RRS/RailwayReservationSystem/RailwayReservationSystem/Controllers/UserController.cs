using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RailwayReservationSystem.Data;
using RailwayReservationSystem.Data.Repository;
using RailwayReservationSystem.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RailwayReservationSystem.Controllers
{
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _repo;

        

        
        public UserController(IConfiguration configuration, IUserRepository repo)
        {
            _configuration = configuration;
            _repo = repo;
        }

        #region "Login Functionality"
        [HttpPost]
        [Route("Login")]
        public ActionResult Login([FromBody] Login login)
        {
            User user = _repo.CheckUser(login);
            if(user == null)
                return NotFound(new { msg = "User Not Found..."});

            var token = GenerateJWT(user);
            return Ok(token);

        }

        #region "Generate JWT Token"
        private string GenerateJWT(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var credintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Gender, user.Gender),
                new Claim(ClaimTypes.DateOfBirth, user.Dob.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var securityToken = new JwtSecurityToken(_configuration["JWT:issuer"],
                                                    _configuration["JWT:audience"],
                                                    claims,
                                                    expires: DateTime.Now.AddMinutes(15),
                                                    signingCredentials: credintials);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
        #endregion
        #endregion

        #region "Register Functionality"
        [HttpPost]
        [Route("Register")]
        public ActionResult Register([FromBody]Register reg)
        {
            if (ModelState.IsValid)
            {
                if (_repo.CheckEmail(reg))
                {
                    User user = _repo.AddUser(reg);
                    return CreatedAtAction("Register", user);
                }
                else
                {
                    return Conflict(new { msg = "Email Already Exists..." });
                }
            }
            else
            {
                return ValidationProblem("Check all fields");
            }
        }
        #endregion
    }
}
