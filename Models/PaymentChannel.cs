using System.ComponentModel.DataAnnotations;

namespace RoomReservationSystem.Models
{
    public class PaymentChannel
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public string? AccountNumber { get; set; }
        
        public string? AccountName { get; set; }
        
        public string? BankName { get; set; }
        
        public string? QRCodeUrl { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
