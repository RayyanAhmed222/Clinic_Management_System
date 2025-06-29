using System;
using System.Collections.Generic;

namespace Clinic_Management_System.Models
{
    public partial class Bank
    {
        public int AccId { get; set; }
        public string AccName { get; set; } = null!;
        public string AccNumber { get; set; } = null!;
    }
}
