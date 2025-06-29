namespace Clinic_Management_System.Models
{
    public class OrderScientificFormViewModel
    {
        public int SciId { get; set; }
        public string Type { get; set; }

        public ScientificInfo Scientific { get; set; }
        public SellScientificInfo SellScientific { get; set; }

        public ScientificOrder ScientificOrder { get; set; }
        public List<Bank> BankDetails { get; set; }
    }

}
