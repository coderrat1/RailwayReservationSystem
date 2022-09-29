using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using RailwayReservationSystem.Data;
using RailwayReservationSystem.Data.Repository;
using RailwayReservationSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace RailwayReservationSystem.Controllers
{
    [Route("Book")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _resRepo;
        private readonly IPassengerRepository _passRepo;
        private readonly ILogger<Reservation> _log;

        public ReservationController(IReservationRepository resRepo, IPassengerRepository passRepo, ILogger<Reservation> log)
        {
            _resRepo = resRepo;
            _passRepo = passRepo;
            _log = log;
        }

        #region "Reservation & Payment"
        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("Pay")]
        public IActionResult BookNow([FromBody] ReservationDto dto)
        {

            Ticket ticket = null;
            if (ModelState.IsValid)
            {
                if(dto.Passengers.Count > 6)
                {
                    return BadRequest(new { msg = "Maximum 6 Passenger allowed in 1 booking..." });
                }
                
                int userId = 1000;//////
                try
                {
                    List<Passenger> passes = _passRepo.AddPassenger(dto.Passengers, userId);
                    
                    if (passes.Count > 0)
                    {
                        ticket = _resRepo.AddReservation(dto, userId, passes);
                    }
                }
                catch(Exception error)
                {
                    _log.LogError(error.Message);
                    return BadRequest(error.Message);
                } 
            }

            if(ticket != null)
                return Ok(ticket);
            return BadRequest(new { msg = "Something went wrong..." });
        }
        #endregion


        #region "Get Ticket"
        [Authorize(Roles = "User")]
        [HttpGet("GetTicket/{pnr}")]
        public ActionResult GetReservation(int pnr)
        {
            Ticket ticket = _resRepo.GetTicket(pnr);



            if (ticket == null)
            {
                return NotFound(new {msg="Please Enter a Valid PNR Number...."});
            }

            return Ok(ticket);
        }
        #endregion


        #region "Cancel Ticket"
        [Authorize(Roles = "User")]
        [HttpPut]
        [Route("Cancel/{pnr}")]
        public ActionResult CancelTicket(int pnr)
        {
            Ticket ticket = _resRepo.CancelTicket(pnr);



            if (ticket == null)
            {
                return NotFound(new { msg = "Cancellation Failed...." });
            }

            return Ok(ticket);
        }
        #endregion
    }
}
