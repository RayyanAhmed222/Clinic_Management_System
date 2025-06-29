using System;
using System.Collections.Generic;

namespace Clinic_Management_System.Models
{
    public partial class MedicinesCategory
    {
        public MedicinesCategory()
        {
            MedicinesInfos = new HashSet<MedicinesInfo>();
            SellMedicinesInfos = new HashSet<SellMedicinesInfo>();
        }

        public int MedCatId { get; set; }
        public string MedCatName { get; set; } = null!;

        public virtual ICollection<MedicinesInfo> MedicinesInfos { get; set; }
        public virtual ICollection<SellMedicinesInfo> SellMedicinesInfos { get; set; }
    }
}
