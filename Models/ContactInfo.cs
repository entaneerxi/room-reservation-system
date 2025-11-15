using System.ComponentModel.DataAnnotations;

namespace RoomReservationSystem.Models
{
    public class ContactInfo
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Address { get; set; } = string.Empty;
        
        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;
        
        [EmailAddress]
        public string? Email { get; set; }
        
        public string? Line { get; set; }
        
        public string? Facebook { get; set; }
        
        public string? MapUrl { get; set; }
        
        public string? WorkingHours { get; set; }
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
