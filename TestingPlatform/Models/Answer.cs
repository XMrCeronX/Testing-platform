using System;
using System.Collections.Generic;

namespace TestingPlatform.Models
{
    public partial class Answer
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public string Text { get; set; } = null!;
        public bool IsTrueAnswer { get; set; }

        public virtual Question Question { get; set; } = null!;
    }
}
