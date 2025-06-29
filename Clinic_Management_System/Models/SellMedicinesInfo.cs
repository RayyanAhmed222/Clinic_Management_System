using System;
using System.Collections.Generic;

namespace Clinic_Management_System.Models
{
    public partial class SellMedicinesInfo
    {
        public int MedId { get; set; }
        public string MedName { get; set; } = null!;
        public string MedBrandName { get; set; } = null!;
        public string MedDescription { get; set; } = null!;
        public string? MedImage { get; set; }
        public string MedPrice { get; set; } = null!;
        public string StockStatus { get; set; } = null!;
        public int? MedCat { get; set; }

        public virtual MedicinesCategory? MedCatNavigation { get; set; }
    }
}
