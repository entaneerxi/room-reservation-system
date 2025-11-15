using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomReservationSystem.Models
{
    public class MonthlyRental
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public int RoomId { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        [Required]
        public decimal MonthlyRent { get; set; }
        
        public string Status { get; set; } = "Active"; // Active, Ended
        
        public string? TenantNotes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        
        [ForeignKey("RoomId")]
        public Room? Room { get; set; }
        
        public ICollection<UtilityBill>? UtilityBills { get; set; }
        public ICollection<Payment>? Payments { get; set; }
    }
}
