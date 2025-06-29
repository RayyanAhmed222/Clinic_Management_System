using System;
using System.Collections.Generic;

namespace Clinic_Management_System.Models
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public string? ClientName { get; set; }
        public string? ProductName { get; set; }
        public string? ProductImg { get; set; }
        public string? ClientFeedback { get; set; }
    }
}
