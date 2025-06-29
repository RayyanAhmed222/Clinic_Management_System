namespace Clinic_Management_System.Models
{
    public class AddMedViewModel
    {
        public List<MedicinesCategory> MedCategoryList { get; set; }

        public MedicinesCategory SelectedCategory { get; set; }

        public MedicinesInfo medicineinfo { get; set; }

    }
}
