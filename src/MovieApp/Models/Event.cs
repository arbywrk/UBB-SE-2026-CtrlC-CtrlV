using System;
using System.ComponentModel.DataAnnotations;

namespace MovieApp.Models
{
    public class Event
    {
        [Key] // Tells SQL this is the unique ID
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public string Location { get; set; } = string.Empty;

        [Range(0, double.MaxValue)] 
        public decimal TicketPrice { get; set; }

        public double HistoricalRating { get; set; }
        public string EventType { get; set; } = "Movie"; // Default value
        public int Capacity { get; set; }
        public string CreatorId { get; set; } = string.Empty;
    }
}