using System;
using System.Collections.Generic;

namespace Clinic_Management_System.Models
{
    public partial class MedicineOrder
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public string MedName { get; set; } = null!;
        public string MedCategory { get; set; } = null!;
        public string MedPrice { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserAddress { get; set; } = null!;
        public string? TransactionScreenshot { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
