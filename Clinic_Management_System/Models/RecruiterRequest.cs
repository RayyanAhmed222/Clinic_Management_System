using System;
using System.Collections.Generic;

namespace Clinic_Management_System.Models
{
    public partial class RecruiterRequest
    {
        public int RequestId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Reason { get; set; } = null!;
        public DateTime? RequestDate { get; set; }
        public string? Status { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
