using System;
using System.Collections.Generic;

namespace Clinic_Management_System.Models
{
    public partial class PracticalCategory
    {
        public PracticalCategory()
        {
            Practicals = new HashSet<Practical>();
        }

        public int PracCatId { get; set; }
        public string PracCatName { get; set; } = null!;

        public virtual ICollection<Practical> Practicals { get; set; }
    }
}
