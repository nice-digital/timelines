using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NICE.Timelines.DB.Models
{
    public class TimelinesContext : DbContext
    {
        public TimelinesContext()
        { }

        public TimelinesContext(DbContextOptions options) : base(options)
        { }

        public virtual DbSet<TimelineTask> TimelineTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfiguration Configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
                    .Build();

                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<TimelineTask>(entity =>
            {
                entity.ToTable("TimelineTask");

                entity.Property(e => e.ACID).HasColumnName("ACID");

                entity.Property(e => e.PhaseId).IsRequired();

                entity.Property(e => e.ClickUpSpaceId).HasMaxLength(255);
                entity.Property(e => e.ClickUpFolderId).HasMaxLength(255);
                entity.Property(e => e.ClickUpListId).HasMaxLength(255);
                entity.Property(e => e.ClickUpTaskId).HasMaxLength(255);

                entity.HasOne(e => e.Phase)
                    .WithMany(e => e.TimelineTasks)
                    .HasForeignKey(e => e.PhaseId)
                    .HasConstraintName("TimelineTasks_Stage")
                    .IsRequired();
            });

            modelBuilder.Entity<Phase>().HasData(
                new { PhaseId = 12, PhaseDescription = "Invitation to participate" },
                new { PhaseId = 13, PhaseDescription = "Submissions" },
                new { PhaseId = 14, PhaseDescription = "Assessment report" },
                new { PhaseId = 15, PhaseDescription = "Assessment report consultation" },
                new { PhaseId = 16, PhaseDescription = "Overview" },
                new { PhaseId = 17, PhaseDescription = "Evidence critique" },
                new { PhaseId = 18, PhaseDescription = "Pre meeting briefing" },
                new { PhaseId = 19, PhaseDescription = "First committee meeting" },
                new { PhaseId = 20, PhaseDescription = "Consultation" },
                new { PhaseId = 21, PhaseDescription = "FAD sign off" },
                new { PhaseId = 22, PhaseDescription = "FAD appeal period" },
                new { PhaseId = 24, PhaseDescription = "Publication" },
                new { PhaseId = 26, PhaseDescription = "Committee meeting" },
                new { PhaseId = 27, PhaseDescription = "Scope review" },
                new { PhaseId = 28, PhaseDescription = "Consultee meeting" },
                new { PhaseId = 109, PhaseDescription = "Scoping" },
                new { PhaseId = 113, PhaseDescription = "Technical report" });
        }
    }
}
