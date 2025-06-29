using System;
using System.Collections.Generic;

namespace Clinic_Management_System.Models
{
    public partial class Category
    {
        public int CategorysId { get; set; }
        public string CategorysName { get; set; } = null!;
        public string? CategorysImg { get; set; }
    }
}
