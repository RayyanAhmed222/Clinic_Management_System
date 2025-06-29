namespace Clinic_Management_System.Models
{
    public class AddSellMedViewModel
    {
        public List<MedicinesCategory> medcategory { get; set; }

        public MedicinesCategory SelectedCategory { get; set; }

        public SellMedicinesInfo sellmedicineinfo { get; set; }
    }
}
