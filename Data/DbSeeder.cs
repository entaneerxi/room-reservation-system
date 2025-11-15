using Microsoft.AspNetCore.Identity;
using RoomReservationSystem.Models;

namespace RoomReservationSystem.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Seed Roles
            string[] roleNames = { "Admin", "Staff", "Member" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed Admin User
            var adminEmail = "admin@roomreservation.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.Now
                };
                await userManager.CreateAsync(adminUser, "Admin@123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Seed Staff User
            var staffEmail = "staff@roomreservation.com";
            var staffUser = await userManager.FindByEmailAsync(staffEmail);
            if (staffUser == null)
            {
                staffUser = new ApplicationUser
                {
                    UserName = staffEmail,
                    Email = staffEmail,
                    FirstName = "Staff",
                    LastName = "User",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.Now
                };
                await userManager.CreateAsync(staffUser, "Staff@123");
                await userManager.AddToRoleAsync(staffUser, "Staff");
            }

            // Seed Contact Info
            if (!context.ContactInfos.Any())
            {
                context.ContactInfos.Add(new ContactInfo
                {
                    Name = "ระบบจองห้องพัก",
                    Address = "123 ถนนสุขุมวิท แขวงคลองเตย เขตคลองเตย กรุงเทพมหานคร 10110",
                    Phone = "02-123-4567",
                    Email = "info@roomreservation.com",
                    Line = "@roomreservation",
                    Facebook = "https://facebook.com/roomreservation",
                    WorkingHours = "จันทร์-อาทิตย์ 08:00-20:00 น.",
                    UpdatedAt = DateTime.Now
                });
            }

            // Seed Facilities
            if (!context.Facilities.Any())
            {
                var facilities = new List<Facility>
                {
                    new Facility { Name = "Free WiFi", Description = "อินเทอร์เน็ตความเร็วสูง", IconClass = "fas fa-wifi", DisplayOrder = 1, IsActive = true, CreatedAt = DateTime.Now },
                    new Facility { Name = "ที่จอดรถ", Description = "ที่จอดรถฟรี", IconClass = "fas fa-parking", DisplayOrder = 2, IsActive = true, CreatedAt = DateTime.Now },
                    new Facility { Name = "เครื่องซักผ้า", Description = "เครื่องซักผ้าหยอดเหรียญ", IconClass = "fas fa-tshirt", DisplayOrder = 3, IsActive = true, CreatedAt = DateTime.Now },
                    new Facility { Name = "แอร์", Description = "เครื่องปรับอากาศ", IconClass = "fas fa-snowflake", DisplayOrder = 4, IsActive = true, CreatedAt = DateTime.Now },
                    new Facility { Name = "ทีวี", Description = "โทรทัศน์", IconClass = "fas fa-tv", DisplayOrder = 5, IsActive = true, CreatedAt = DateTime.Now },
                    new Facility { Name = "ตู้เย็น", Description = "ตู้เย็นในห้อง", IconClass = "fas fa-box", DisplayOrder = 6, IsActive = true, CreatedAt = DateTime.Now },
                };
                context.Facilities.AddRange(facilities);
            }

            // Seed Rooms
            if (!context.Rooms.Any())
            {
                var rooms = new List<Room>
                {
                    new Room
                    {
                        Name = "Standard Room 101",
                        Description = "ห้องพักมาตรฐาน พร้อมสิ่งอำนวยความสะดวกครบครัน",
                        DailyRate = 500,
                        MonthlyRate = 8000,
                        Capacity = 2,
                        RoomType = "Standard",
                        IsAvailable = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Room
                    {
                        Name = "Standard Room 102",
                        Description = "ห้องพักมาตรฐาน พร้อมสิ่งอำนวยความสะดวกครบครัน",
                        DailyRate = 500,
                        MonthlyRate = 8000,
                        Capacity = 2,
                        RoomType = "Standard",
                        IsAvailable = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Room
                    {
                        Name = "Deluxe Room 201",
                        Description = "ห้องพักดีลักซ์ พื้นที่กว้างขวาง พร้อมระเบียง",
                        DailyRate = 800,
                        MonthlyRate = 12000,
                        Capacity = 2,
                        RoomType = "Deluxe",
                        IsAvailable = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Room
                    {
                        Name = "Deluxe Room 202",
                        Description = "ห้องพักดีลักซ์ พื้นที่กว้างขวาง พร้อมระเบียง",
                        DailyRate = 800,
                        MonthlyRate = 12000,
                        Capacity = 2,
                        RoomType = "Deluxe",
                        IsAvailable = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Room
                    {
                        Name = "Suite Room 301",
                        Description = "ห้องสวีท หรูหรา พื้นที่กว้างขวาง เหมาะสำหรับครอบครัว",
                        DailyRate = 1500,
                        MonthlyRate = 20000,
                        Capacity = 4,
                        RoomType = "Suite",
                        IsAvailable = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Room
                    {
                        Name = "Suite Room 302",
                        Description = "ห้องสวีท หรูหรา พื้นที่กว้างขวาง เหมาะสำหรับครอบครัว",
                        DailyRate = 1500,
                        MonthlyRate = 20000,
                        Capacity = 4,
                        RoomType = "Suite",
                        IsAvailable = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    }
                };
                context.Rooms.AddRange(rooms);
            }

            // Seed Promotions
            if (!context.Promotions.Any())
            {
                var promotions = new List<Promotion>
                {
                    new Promotion
                    {
                        Title = "ส่วนลดต้อนรับปีใหม่",
                        Description = "จองห้องพักรับส่วนลด 20% สำหรับการจองในเดือนมกราคม",
                        DiscountPercentage = 20,
                        StartDate = new DateTime(2025, 1, 1),
                        EndDate = new DateTime(2025, 1, 31),
                        PromoCode = "NEWYEAR2025",
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Promotion
                    {
                        Title = "โปรโมชั่นเช่าระยะยาว",
                        Description = "เช่าห้องพักรายเดือน ลดทันที 15%",
                        DiscountPercentage = 15,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddMonths(3),
                        PromoCode = "LONGTERM15",
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Promotion
                    {
                        Title = "สุดสัปดาห์สุดคุ้ม",
                        Description = "จองห้องพักช่วงสุดสัปดาห์ ลด 10%",
                        DiscountPercentage = 10,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddMonths(6),
                        PromoCode = "WEEKEND10",
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    }
                };
                context.Promotions.AddRange(promotions);
            }

            // Seed Payment Channels
            if (!context.PaymentChannels.Any())
            {
                var paymentChannels = new List<PaymentChannel>
                {
                    new PaymentChannel
                    {
                        Name = "โอนเงินผ่านธนาคาร",
                        Description = "โอนเงินผ่านบัญชีธนาคาร",
                        BankName = "ธนาคารกสิกรไทย",
                        AccountName = "ระบบจองห้องพัก",
                        AccountNumber = "123-4-56789-0",
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new PaymentChannel
                    {
                        Name = "PromptPay",
                        Description = "โอนเงินผ่าน QR Code PromptPay",
                        AccountNumber = "0812345678",
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new PaymentChannel
                    {
                        Name = "ชำระเงินสด",
                        Description = "ชำระเงินสดที่สำนักงาน",
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    }
                };
                context.PaymentChannels.AddRange(paymentChannels);
            }

            // Seed Additional Options
            if (!context.AdditionalOptions.Any())
            {
                var options = new List<AdditionalOption>
                {
                    new AdditionalOption
                    {
                        Name = "ที่นอนเสริม",
                        Description = "ที่นอนเสริมสำหรับผู้เข้าพักเพิ่มเติม",
                        Price = 200,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new AdditionalOption
                    {
                        Name = "บริการรถรับส่ง",
                        Description = "บริการรับส่งสนามบิน",
                        Price = 500,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new AdditionalOption
                    {
                        Name = "อาหารเช้า",
                        Description = "บริการอาหารเช้าในห้อง",
                        Price = 100,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    }
                };
                context.AdditionalOptions.AddRange(options);
            }

            await context.SaveChangesAsync();
        }
    }
}
