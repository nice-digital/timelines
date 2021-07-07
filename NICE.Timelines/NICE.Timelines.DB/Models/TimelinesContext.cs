using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NICE.Timelines.DB.Models
{
    public class TimelinesContext : DbContext
    {
        public TimelinesContext()
        { }

        public TimelinesContext(DbContextOptions<TimelinesContext> options) : base(options)
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

                entity.Property(e => e.StepDescription)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.StageDescription)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ClickUpSpaceId).HasMaxLength(255);
                entity.Property(e => e.ClickUpFolderId).HasMaxLength(255);
                entity.Property(e => e.ClickUpListId).HasMaxLength(255);
                entity.Property(e => e.ClickUpTaskId).HasMaxLength(255);
            });
        }
    }
}
