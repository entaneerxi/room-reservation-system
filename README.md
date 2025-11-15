# Room Reservation System
ระบบจองห้องพัก เช่าห้องพัก

## Technology Stack
- **ASP.NET Core 8 MVC** - C#
- **Microsoft SQL Server** - Database
- **Entity Framework Core** - ORM
- **ASP.NET Core Identity** - Authentication & Authorization
- **Bootstrap 5** - CSS Framework
- **AdminLTE 3** - Admin Panel Theme
- **jQuery** - JavaScript Library
- **SweetAlert2** - Alert Notifications
- **Font Awesome 6** - Icons

## Features

### Frontend (User Side)
- **หน้าแรก** - Homepage with sliders, promotions, and room listings
- **จองห้องพัก** - Room booking (daily and monthly)
- **โปรโมชั่น** - Promotions page
- **ภาพบรรยากาศ** - Gallery
- **สิ่งอำนวยความสะดวก** - Facilities page
- **ติดต่อเรา** - Contact page
- **สมัครสมาชิก/เข้าสู่ระบบ** - User registration and login
- **การจองของฉัน** - Booking history
- **เลื่อนการจอง** - Reschedule bookings
- **ยกเลิกการจอง** - Cancel bookings

### Admin Backend
#### Dashboard
- Overview statistics
- Recent bookings
- Room availability status

#### Booking Management (การจัดการการจอง)
- รายงานข้อมูลการจอง - Booking reports
- ยืนยันการชำระเงิน - Confirm payments
- ยืนยันการจอง - Confirm bookings
- เช็คอิน - Check-in guests
- เช็คเอาท์ - Check-out guests
- ข้อมูลการจอง - Booking details

#### Monthly Rental Management (การจัดการเช่ารายเดือน)
- ข้อมูลการเช่า - Rental information
- ข้อมูลผู้เช่าอาศัย - Tenant information
- บันทึกข้อมูลผู้เช่าอาศัย - Record tenant details
- ชำระค่าน้ำและค่าไฟ - Utility bill payments
- ข้อมูลค่าน้ำและค่าไฟ - Utility bill records

#### Data Management (จัดการข้อมูล)
- จัดการห้องพัก (CRUD) - Rooms management
- จัดการโปรโมชั่น (CRUD) - Promotions management
- จัดการสิ่งอำนวยความสะดวก (CRUD) - Facilities management
- จัดการภาพบรรยากาศ (CRUD) - Gallery management
- จัดการสไลด์โฆษณา (CRUD) - Sliders management
- จัดการข้อมูลการติดต่อ (CRUD) - Contact info management
- จัดการช่องทางการชำระเงิน (CRUD) - Payment channels management
- จัดการตัวเลือกเพิ่มเติม (CRUD) - Additional options management

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server or SQL Server LocalDB
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
```bash
git clone https://github.com/entaneerxi/room-reservation-system.git
cd room-reservation-system
```

2. Update the connection string in `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RoomReservationDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

3. Create the database and run migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. Run the application
```bash
dotnet run
```

5. Open your browser and navigate to `https://localhost:5001` or the port shown in the console

### Default Admin Credentials
- **Email:** admin@roomreservation.com
- **Password:** Admin@123

### Project Structure
```
RoomReservationSystem/
├── Areas/
│   └── Admin/          # Admin area with controllers and views
│       ├── Controllers/
│       └── Views/
├── Controllers/        # Main controllers
├── Data/              # DbContext
├── Models/            # Entity models
├── ViewModels/        # View models for forms
├── Views/             # Razor views
│   ├── Home/
│   ├── Account/
│   ├── Booking/
│   └── Shared/
└── wwwroot/           # Static files (CSS, JS, images)
```

## Database Models

### Core Models
- **ApplicationUser** - User accounts with Identity
- **Room** - Room information and rates
- **Booking** - Booking records (daily/monthly)
- **Payment** - Payment transactions
- **MonthlyRental** - Monthly rental contracts
- **UtilityBill** - Water and electricity bills

### Content Models
- **Promotion** - Promotional offers
- **Facility** - Hotel facilities
- **Gallery** - Photo gallery
- **Slider** - Homepage sliders
- **ContactInfo** - Contact information
- **PaymentChannel** - Payment methods
- **AdditionalOption** - Extra services/options

## User Roles
1. **Admin** - Full system access
2. **Staff** - Booking and rental management
3. **Member** - Regular users who can make bookings

## License
This project is licensed under the MIT License.

## Support
For support, email support@roomreservation.com or contact the development team.
