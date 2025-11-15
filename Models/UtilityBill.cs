using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomReservationSystem.Models
{
    public class UtilityBill
    {
        public int Id { get; set; }
        
        [Required]
        public int MonthlyRentalId { get; set; }
        
        [Required]
        public int Month { get; set; }
        
        [Required]
        public int Year { get; set; }
        
        public decimal WaterReading { get; set; }
        
        public decimal ElectricityReading { get; set; }
        
        public decimal WaterAmount { get; set; }
        
        public decimal ElectricityAmount { get; set; }
        
        [Required]
        public decimal TotalAmount { get; set; }
        
        public string Status { get; set; } = "Unpaid"; // Unpaid, Paid
        
        public DateTime? PaidDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        [ForeignKey("MonthlyRentalId")]
        public MonthlyRental? MonthlyRental { get; set; }
    }
}
