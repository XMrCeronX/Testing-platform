using System;
using System.Collections.Generic;

namespace TestingPlatform.Models
{
    public partial class User
    {
        public User()
        {
            UserTests = new HashSet<UserTest>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Patronymic { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public long RoleId { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeLastModified { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<UserTest> UserTests { get; set; }
    }
}
