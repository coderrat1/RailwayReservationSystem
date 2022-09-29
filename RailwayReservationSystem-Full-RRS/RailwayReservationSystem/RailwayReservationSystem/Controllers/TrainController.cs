using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayReservationSystem.Data;
using RailwayReservationSystem.Data.Repository;
using RailwayReservationSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RailwayReservationSystem.Controllers
{
    [Route("Train")]
    public class TrainController : ControllerBase
    {
        private readonly ITrainRepository _repo;

        public TrainController(ITrainRepository repo)
        {
            _repo = repo;
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("Search")]
        public IActionResult SearchTrain([FromBody] SearchTrain search)
        {

            if (ModelState.IsValid)
            {
                List<Train> trains = _repo.SearchTrainDetails(search);
                if (trains != null)
                {
                    return Ok(trains);
                }
                return NotFound(new { msg = "No train for this Search..." });
            }

            return ValidationProblem("Fill the data Properly...");
        }


        //[HttpGet]
        //[Route("ViewTrain")]
        //public async Task<ActionResult<IEnumerable<Train>>> GetTrains()
        //{
        //    return Ok(await _context.Trains.ToListAsync());
        //}


        //[HttpGet("ViewTrain/{id}")]
        //public async Task<ActionResult<Train>> GetTrain(int id)
        //{
        //    var train = await _context.Trains.FindAsync(id);

        //    if (train == null)
        //    {
        //        return NotFound(new { msg = "Train Not available for this Search" });
        //    }

        //    return Ok(train);
        //}

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route(("Update"))]
        public IActionResult UpdateTrain([FromBody] TrainDtoput trainDtoput)
        {
            if (ModelState.IsValid)
            {
                Train train = _repo.UpdateTrainDetails(trainDtoput);

                if (train == null)
                {
                    return NotFound(new { msg = "No Train Found...." });
                }

                return Ok(train);
            }

            return ValidationProblem("Fill the data Properly...");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddTrain")]
        public ActionResult<Train> AddTrain([FromBody] TrainDto trainDto)
        {
            if (ModelState.IsValid)
            {
                Train train = _repo.AddTrainDetails(trainDto);
                if (train == null)
                    return Conflict(new { msg = "Some error happens...Try Again" });
                return CreatedAtAction("AddTrain", train);
            }

            return ValidationProblem("Fill the data Properly...");
        }
    }
}
