using System;
using System.Collections.Generic;

namespace Clinic_Management_System.Models
{
    public partial class Seminar
    {
        public int SemId { get; set; }
        public string SemName { get; set; } = null!;
        public string? SemImg { get; set; }
        public string? SemShortDescription { get; set; }
        public string? SemDescription { get; set; }
        public DateTime? SemDatetime { get; set; }
        public string? SemDuration { get; set; }
        public string? SemStockstatus { get; set; }
        public decimal? SemPrice { get; set; }
        public int? SemFaculty { get; set; }

        public virtual Faculty? SemFacultyNavigation { get; set; }
    }
}
