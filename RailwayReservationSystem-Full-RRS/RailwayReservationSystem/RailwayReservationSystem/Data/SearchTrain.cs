using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using RailwayReservationSystem.Models;

namespace RailwayReservationSystem.Data
{
    public class SearchTrain
    {
        public string From { get; set; }
        public string To { get; set; }
        [DataType(DataType.Date)]
        public DateTime TrainDate { get; set; }
    }
}