using Microsoft.AspNetCore.Identity;

namespace RoomReservationSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? IDCardNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<MonthlyRental>? MonthlyRentals { get; set; }
    }
}
