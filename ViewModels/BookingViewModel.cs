using System.ComponentModel.DataAnnotations;
using RoomReservationSystem.Models;

namespace RoomReservationSystem.ViewModels
{
    public class BookingViewModel
    {
        [Required(ErrorMessage = "กรุณาเลือกห้องพัก")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "กรุณาเลือกวันเข้าพัก")]
        [Display(Name = "วันเข้าพัก")]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "กรุณาเลือกวันออก")]
        [Display(Name = "วันออก")]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        [Required(ErrorMessage = "กรุณาเลือกประเภทการจอง")]
        [Display(Name = "ประเภทการจอง")]
        public string BookingType { get; set; } = "Daily";

        [Display(Name = "รหัสโปรโมชั่น")]
        public string? PromoCode { get; set; }

        [Display(Name = "หมายเหตุ")]
        public string? Notes { get; set; }

        [Display(Name = "ตัวเลือกเพิ่มเติม")]
        public List<int> SelectedAdditionalOptions { get; set; } = new List<int>();

        public Room? Room { get; set; }
        public List<AdditionalOption>? AvailableOptions { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
    }
}
