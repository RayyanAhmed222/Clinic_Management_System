using System;
using System.Collections.Generic;

namespace Clinic_Management_System.Models
{
    public partial class User
    {
        public User()
        {
            RecruiterRequests = new HashSet<RecruiterRequest>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<RecruiterRequest> RecruiterRequests { get; set; }
    }
}
