using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomReservationSystem.Models
{
    public class Payment
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        public int? BookingId { get; set; }
        
        public int? MonthlyRentalId { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        public string PaymentType { get; set; } = "Booking"; // Booking, Utilities, MonthlyRent
        
        [Required]
        public string PaymentMethod { get; set; } = string.Empty;
        
        public string? TransactionId { get; set; }
        
        public string? PaymentSlipUrl { get; set; }
        
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Rejected
        
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        
        public DateTime? ConfirmedDate { get; set; }
        
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        
        [ForeignKey("BookingId")]
        public Booking? Booking { get; set; }
        
        [ForeignKey("MonthlyRentalId")]
        public MonthlyRental? MonthlyRental { get; set; }
    }
}
