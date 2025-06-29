using System;
using System.Collections.Generic;

namespace Clinic_Management_System.Models
{
    public partial class Lecture
    {
        public int LecId { get; set; }
        public string LecName { get; set; } = null!;
        public string? LecImg { get; set; }
        public string? LecShortDescription { get; set; }
        public string? LecDescription { get; set; }
        public DateTime? LecDatetime { get; set; }
        public string? LecDuration { get; set; }
        public string? LecStockstatus { get; set; }
        public decimal? LecPrice { get; set; }
        public int? LecFaculty { get; set; }

        public virtual Faculty? LecFacultyNavigation { get; set; }
    }
}
