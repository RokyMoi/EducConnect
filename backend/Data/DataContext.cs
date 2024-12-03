using backend.Entities.Learning;
using backend.Entities.Person;
using backend.Entities.Reference.Country;
using EduConnect.Entities.Person;
using EduConnect.Entities.Student;
using EduConnect.Entities.Tutor;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{

    public DbSet<Person> Person { get; set; }
    public DbSet<PersonDetails> PersonDetails { get; set; }
    public DbSet<PersonEmail> PersonEmail { get; set; }
    public DbSet<PersonPassword> PersonPassword { get; set; }
    public DbSet<PersonProfilePicture> PersonProfilePicture { get; set; }
    public DbSet<PersonSalt> PersonSalt { get; set; }
    public DbSet<PersonVerificationCode> PersonVerificationCode { get; set; }
    public DbSet<Tutor> Tutor { get; set; }


    public DbSet<Student> Student { get; set; }
    public DbSet<StudentDetails> StudentDetails { get; set; }

    public DbSet<Country> Country { get; set; }

    public DbSet<LearningCategory> LearningCategory { get; set; }
    public DbSet<LearningSubcategory> LearningSubCategory { get; set; }

    public DbSet<PersonEducationInformation> PersonEducationInformation { get; set; }

}