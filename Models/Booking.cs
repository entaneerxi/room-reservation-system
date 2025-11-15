using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomReservationSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public int RoomId { get; set; }
        
        [Required]
        public DateTime CheckInDate { get; set; }
        
        [Required]
        public DateTime CheckOutDate { get; set; }
        
        [Required]
        public string BookingType { get; set; } = "Daily"; // Daily or Monthly
        
        [Required]
        public decimal TotalAmount { get; set; }
        
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, CheckedIn, CheckedOut, Cancelled
        
        public bool PaymentConfirmed { get; set; } = false;
        
        public DateTime? PaymentDate { get; set; }
        
        public DateTime? ActualCheckInDate { get; set; }
        
        public DateTime? ActualCheckOutDate { get; set; }
        
        public int? PromotionId { get; set; }
        
        public decimal? DiscountAmount { get; set; }
        
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        
        [ForeignKey("RoomId")]
        public Room? Room { get; set; }
        
        [ForeignKey("PromotionId")]
        public Promotion? Promotion { get; set; }
        
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<AdditionalOption>? AdditionalOptions { get; set; }
    }
}
