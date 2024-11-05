using Microsoft.EntityFrameworkCore;
using TestingPlatform.Infrastructure.Context;

namespace TestingPlatform.Models
{
    public partial class test_platformContext : DbContext
    {
        public test_platformContext()
        {
        }

        public test_platformContext(DbContextOptions<test_platformContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Test> Tests { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserTest> UserTests { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql(ApplicationContext.GetConnectionString(), Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.39-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("answers");

                entity.HasIndex(e => e.QuestionId, "question_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsTrueAnswer).HasColumnName("is_true_answer");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.Text)
                    .HasMaxLength(255)
                    .HasColumnName("text");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("answers_ibfk_1");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("questions");

                entity.HasIndex(e => e.TestId, "test_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MultipleAnswer).HasColumnName("multiple_answer");

                entity.Property(e => e.NumberOfAnswers).HasColumnName("number_of_answers");

                entity.Property(e => e.TestId).HasColumnName("test_id");

                entity.Property(e => e.Text)
                    .HasMaxLength(255)
                    .HasColumnName("text");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("questions_ibfk_1");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.HasIndex(e => e.Name, "name")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.ToTable("tests");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(512)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.NumberOfQuestions).HasColumnName("number_of_questions");

                entity.Property(e => e.TestStartDatetime)
                    .HasColumnType("timestamp")
                    .HasColumnName("test_start_datetime");

                entity.Property(e => e.TestStopDatetime)
                    .HasColumnType("timestamp")
                    .HasColumnName("test_stop_datetime");

                entity.Property(e => e.TimeCreated)
                    .HasColumnType("timestamp")
                    .HasColumnName("time_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.RoleId, "role_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Login)
                    .HasMaxLength(255)
                    .HasColumnName("login");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.Patronymic)
                    .HasMaxLength(255)
                    .HasColumnName("patronymic");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.Surname)
                    .HasMaxLength(255)
                    .HasColumnName("surname");

                entity.Property(e => e.TimeCreated)
                    .HasColumnType("timestamp")
                    .HasColumnName("time_created")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.TimeLastModified)
                    .HasColumnType("timestamp")
                    .HasColumnName("time_last_modified")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("users_ibfk_1");
            });

            modelBuilder.Entity<UserTest>(entity =>
            {
                entity.ToTable("user_tests");

                entity.HasIndex(e => e.TestId, "test_id");

                entity.HasIndex(e => e.UserId, "user_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.TestId).HasColumnName("test_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.UserTests)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_tests_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserTests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_tests_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
