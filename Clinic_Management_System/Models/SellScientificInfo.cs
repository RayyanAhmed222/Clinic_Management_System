using System;
using System.Collections.Generic;

namespace Clinic_Management_System.Models
{
    public partial class SellScientificInfo
    {
        public int ScientificId { get; set; }
        public string ScientificName { get; set; } = null!;
        public string ScientificBrandName { get; set; } = null!;
        public string ScientificDescription { get; set; } = null!;
        public string? ScientificImage { get; set; }
        public decimal ScientificPrice { get; set; }
        public string StockStatus { get; set; } = null!;
        public int? ScientificCat { get; set; }

        public virtual ScientificCategory? ScientificCatNavigation { get; set; }
    }
}
