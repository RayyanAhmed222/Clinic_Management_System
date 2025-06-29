namespace Clinic_Management_System.Models
{
    public class AddParcViewModel
    {
        public List<Faculty> FacultyList { get; set; }
        public Practical practical { get; set; }
        public List<PracticalCategory> pracList { get; set; }

        public PracticalCategory SelectedCategory { get; set; }
    }
}
