using System;
using System.Collections.Generic;

namespace Clinic_Management_System.Models
{
    public partial class ScientificOrder
    {
        public int ScientificOrderId { get; set; }
        public int ScientificId { get; set; }
        public string SciName { get; set; } = null!;
        public string SciCategory { get; set; } = null!;
        public string SciPrice { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserAddress { get; set; } = null!;
        public string? TransactionScreenshot { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
