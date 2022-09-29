using System;
using System.ComponentModel.DataAnnotations;

namespace RailwayReservationSystem.Data
{
    public class TrainDtoput
    {
        public int TrainId { get; set; }
        public int TrainNo { get; set; }
        public string TrainName { get; set; }
        public string SourceStation { get; set; }
        public string DestinationStation { get; set; }
        public DateTime SourceDepartureTime { get; set; }
        public DateTime DestinationArrivalTime { get; set; }
        public int TotalSeat { get; set; }
        public int AvailableGeneralSeat { get; set; }
        public int AvailableLadiesSeat { get; set; }
        public decimal SeatFare { get; set; }
    }
}
