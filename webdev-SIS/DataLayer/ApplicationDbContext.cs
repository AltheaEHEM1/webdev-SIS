using Microsoft.EntityFrameworkCore;
using webdev_SIS.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace webdev_SIS.DataLayer
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {

        // 🔹 Correct DbSet naming (plural form)
        //public DbSet<StudentEntity> Section7 { get; set; }
        //public DbSet<SignupFacultyEntity> Signupfaculty { get; set; } 
        //public DbSet<AccountEntity> Accounts { get; set; }

        public DbSet<AnnouncementEntity> Announcements { get; set; }
        public DbSet<UnenrollEntity> UnenrolledStudents { get; set; }
        //public DbSet<ProfileEntity> Profile { get; set; }
        public DbSet<AdmissionEntity> Admissions { get; set; }
        public DbSet<EnrollmentEntity> Enrollment { get; set; }
        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<UserEntity> Users { get; set; }







        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔐 Identity key configuration
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            // 🔧 Decimal precision = (5, 2)

            // AdmissionEntity
            modelBuilder.Entity<AdmissionEntity>().Property(e => e.GWA).HasPrecision(5, 2);
            modelBuilder.Entity<AdmissionEntity>().Property(e => e.Height).HasPrecision(5, 2);
            modelBuilder.Entity<AdmissionEntity>().Property(e => e.Weight).HasPrecision(5, 2);

            // StudentEntity
            modelBuilder.Entity<StudentEntity>().Property(s => s.GWA).HasPrecision(5, 2);
            modelBuilder.Entity<StudentEntity>().Property(s => s.Height).HasPrecision(5, 2);
            modelBuilder.Entity<StudentEntity>().Property(s => s.Weight).HasPrecision(5, 2);

            // EnrollmentEntity
            modelBuilder.Entity<EnrollmentEntity>().Property(e => e.GWA).HasPrecision(5, 2);
            modelBuilder.Entity<EnrollmentEntity>().Property(e => e.Height).HasPrecision(5, 2);
            modelBuilder.Entity<EnrollmentEntity>().Property(e => e.Weight).HasPrecision(5, 2);

            // UnenrollEntity - Enhanced configuration
            modelBuilder.Entity<UnenrollEntity>()
                .HasKey(e => e.Id); // Explicitly define the primary key

            modelBuilder.Entity<UnenrollEntity>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd() // Ensure it's auto-incrementing
                .IsRequired(); // Make sure it's required

            modelBuilder.Entity<UnenrollEntity>().Property(e => e.GWA).HasPrecision(5, 2);
            modelBuilder.Entity<UnenrollEntity>().Property(e => e.Height).HasPrecision(5, 2);
            modelBuilder.Entity<UnenrollEntity>().Property(e => e.Weight).HasPrecision(5, 2);

            // Ensure nullable string properties are properly configured
            modelBuilder.Entity<UnenrollEntity>().Property(e => e.FirstName).IsRequired(false);
            modelBuilder.Entity<UnenrollEntity>().Property(e => e.MiddleName).IsRequired(false);
            modelBuilder.Entity<UnenrollEntity>().Property(e => e.LastName).IsRequired(false);
            modelBuilder.Entity<UnenrollEntity>().Property(e => e.Email).IsRequired(false);
            // Add similar configurations for other nullable string properties if needed

            modelBuilder.Entity<UnenrollEntity>().ToTable("UnenrolledStudents");

        }

    }
}