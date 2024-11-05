using System;
using System.Collections.Generic;

namespace TestingPlatform.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public long Id { get; set; }
        public long TestId { get; set; }
        public string Text { get; set; } = null!;
        public byte NumberOfAnswers { get; set; }
        public bool MultipleAnswer { get; set; }

        public virtual Test Test { get; set; } = null!;
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
