using System.ComponentModel.DataAnnotations;

namespace RoomReservationSystem.Models
{
    public class Slider
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        
        public string? LinkUrl { get; set; }
        
        public int DisplayOrder { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
