using System.ComponentModel.DataAnnotations;

namespace RoomReservationSystem.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "กรุณากรอกอีเมล")]
        [EmailAddress(ErrorMessage = "รูปแบบอีเมลไม่ถูกต้อง")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "กรุณากรอกรหัสผ่าน")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "จดจำฉัน")]
        public bool RememberMe { get; set; }
    }
}
