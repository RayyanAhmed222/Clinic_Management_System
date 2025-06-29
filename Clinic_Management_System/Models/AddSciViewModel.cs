namespace Clinic_Management_System.Models
{
    public class AddSciViewModel
    {
        public List<ScientificCategory> SciCategoryList { get; set; }

        public ScientificInfo scientificinfo { get; set; }

        public ScientificCategory SelectedCategory { get; set; }
    }
}
