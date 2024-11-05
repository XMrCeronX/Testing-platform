using System;
using System.Collections.Generic;

namespace TestingPlatform.Models
{
    public partial class Test
    {
        public Test()
        {
            Questions = new HashSet<Question>();
            UserTests = new HashSet<UserTest>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public byte NumberOfQuestions { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TestStartDatetime { get; set; }
        public DateTime TestStopDatetime { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<UserTest> UserTests { get; set; }
    }
}
