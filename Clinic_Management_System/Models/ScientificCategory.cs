using System;
using System.Collections.Generic;

namespace Clinic_Management_System.Models
{
    public partial class ScientificCategory
    {
        public ScientificCategory()
        {
            ScientificInfos = new HashSet<ScientificInfo>();
            SellScientificInfos = new HashSet<SellScientificInfo>();
        }

        public int SciCatId { get; set; }
        public string SciCatName { get; set; } = null!;

        public virtual ICollection<ScientificInfo> ScientificInfos { get; set; }
        public virtual ICollection<SellScientificInfo> SellScientificInfos { get; set; }
    }
}
