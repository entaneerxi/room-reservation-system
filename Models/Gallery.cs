using System.ComponentModel.DataAnnotations;

namespace RoomReservationSystem.Models
{
    public class Gallery
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        
        public string Category { get; set; } = "General"; // General, Room, Facility, Event
        
        public int DisplayOrder { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
