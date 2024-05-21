using Microsoft.EntityFrameworkCore;
using StudentApi.Entities;

namespace StudentApi.AppDbContext
{
    public class StudentDbcontext : DbContext
    {
        public StudentDbcontext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}