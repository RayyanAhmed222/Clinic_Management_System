namespace Clinic_Management_System.Models
{
    public class OrderMedFormViewModel
    {
        public MedicinesInfo Medicine { get; set; }
        public SellMedicinesInfo SellMedicine { get; set; }

        public MedicineOrder MedicineOrder { get; set; }

        public List<Bank> BankDetails { get; set; } = new List<Bank>();

        public string Type { get; set; }

        public int MedId { get; set; }   // This is critical for POST binding
    }
}

