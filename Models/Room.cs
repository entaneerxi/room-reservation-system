using System.ComponentModel.DataAnnotations;

namespace RoomReservationSystem.Models
{
    public class Room
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public decimal DailyRate { get; set; }
        
        [Required]
        public decimal MonthlyRate { get; set; }
        
        [Required]
        public int Capacity { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public bool IsAvailable { get; set; } = true;
        
        public string RoomType { get; set; } = "Standard"; // Standard, Deluxe, Suite
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<MonthlyRental>? MonthlyRentals { get; set; }
    }
}
