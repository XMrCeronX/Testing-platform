using System;
using System.Collections.Generic;

namespace TestingPlatform.Models
{
    public partial class UserTest
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long TestId { get; set; }

        public virtual Test Test { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
