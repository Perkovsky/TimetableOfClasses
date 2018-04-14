using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace TimetableOfClasses.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherDiscipline> TeacherDisciplines { get; set; }
        public DbSet<Timetable> Timetables { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeacherDiscipline>()
                .HasKey(t => new { t.TeacherId, t.DisciplineId });

            modelBuilder.Entity<TeacherDiscipline>()
                .HasOne(sc => sc.Teacher)
                .WithMany(s => s.TeacherDisciplines)
                .HasForeignKey(sc => sc.TeacherId);

            modelBuilder.Entity<TeacherDiscipline>()
                .HasOne(sc => sc.Discipline)
                .WithMany(c => c.TeacherDisciplines)
                .HasForeignKey(sc => sc.DisciplineId);
        }
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args) =>
            Program.BuildWebHost(args).Services.GetRequiredService<ApplicationDbContext>();
    }
}
