using System.ComponentModel.DataAnnotations;

namespace RoomReservationSystem.Models
{
    public class Facility
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public string? IconClass { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public int DisplayOrder { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
