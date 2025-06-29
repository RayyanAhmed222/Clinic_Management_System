using System;
using System.Collections.Generic;

namespace Clinic_Management_System.Models
{
    public partial class Practical
    {
        public int PracId { get; set; }
        public string PracName { get; set; } = null!;
        public string? PracImg { get; set; }
        public string? PracShortDescription { get; set; }
        public string? PracDescription { get; set; }
        public DateTime? PracDatetime { get; set; }
        public string? ParcDuration { get; set; }
        public decimal? PracPrice { get; set; }
        public string? PracStockstatus { get; set; }
        public int? PracticalFaculty { get; set; }
        public int? PracticalCategory { get; set; }

        public virtual PracticalCategory? PracticalCategoryNavigation { get; set; }
        public virtual Faculty? PracticalFacultyNavigation { get; set; }
    }
}
