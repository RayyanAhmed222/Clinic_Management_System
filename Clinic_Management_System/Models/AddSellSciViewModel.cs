namespace Clinic_Management_System.Models
{
    public class AddSellSciViewModel
    {
        public List<ScientificCategory> SciCategoryList { get; set; }

        public SellScientificInfo sellscientificinfo { get; set; }

        public ScientificCategory SelectedCategory { get; set; }
    }
}
