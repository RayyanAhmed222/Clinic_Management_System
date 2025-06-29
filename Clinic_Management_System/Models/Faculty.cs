using System;
using System.Collections.Generic;

namespace Clinic_Management_System.Models
{
    public partial class Faculty
    {
        public Faculty()
        {
            Lectures = new HashSet<Lecture>();
            Practicals = new HashSet<Practical>();
            Seminars = new HashSet<Seminar>();
        }

        public int FacultyId { get; set; }
        public string FacultyName { get; set; } = null!;
        public string? FacultyGender { get; set; }
        public int? FacultyAge { get; set; }
        public string FacultyEmail { get; set; } = null!;
        public string? FacultyImage { get; set; }
        public string? FacultyQualification { get; set; }
        public string? FacultySpecialization { get; set; }
        public string? FacultyPhone { get; set; }

        public virtual ICollection<Lecture> Lectures { get; set; }
        public virtual ICollection<Practical> Practicals { get; set; }
        public virtual ICollection<Seminar> Seminars { get; set; }
    }
}
