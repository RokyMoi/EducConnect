using backend.Entities.Learning;
using backend.Entities.Person;
using backend.Entities.Reference;
using backend.Entities.Reference.Country;
using EduConnect.Entities.Tutor;
using EduConnect.Entities.Person;
using EduConnect.Entities.Reference;
using EduConnect.Entities.Student;
using Microsoft.EntityFrameworkCore;
using EduConnect.Entities.Messenger;

namespace EduConnect.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{

    public DbSet<Person> Person { get; set; }
    public DbSet<PersonDetails> PersonDetails { get; set; }

    public DbSet<PersonEmail> PersonEmail { get; set; }
    public DbSet<PersonPassword> PersonPassword { get; set; }
    public DbSet<PersonProfilePicture> PersonProfilePicture { get; set; }
    public DbSet<PersonSalt> PersonSalt { get; set; }
    public DbSet<PersonPhoto> PersonPhoto { get; set; }
    public DbSet<PersonVerificationCode> PersonVerificationCode { get; set; }

    public DbSet<PersonPhoneNumber> PersonPhoneNumber { get; set; }
    public DbSet<Tutor> Tutor { get; set; }

    public DbSet<TutorRegistrationStatus> TutorRegistrationStatus { get; set; }
    public DbSet<Student> Student { get; set; }
    public DbSet<Message> Message { get; set; }
    public DbSet<StudentDetails> StudentDetails { get; set; }

    public DbSet<Country> Country { get; set; }

    public DbSet<LearningCategory> LearningCategory { get; set; }
    public DbSet<LearningSubcategory> LearningSubCategory { get; set; }

    public DbSet<PersonEducationInformation> PersonEducationInformation { get; set; }

    public DbSet<EmploymentType> EmploymentType { get; set; }
    public DbSet<WorkType> WorkType { get; set; }

    public DbSet<TutorTeachingStyleType> TutorTeachingStyleType { get; set; }
    public DbSet<PersonCareerInformation> PersonCareerInformation { get; set; }

    public DbSet<PersonAvailability> PersonAvailibility { get; set; }

    public DbSet<CommunicationType> CommunicationType { get; set; }

    public DbSet<EngagementMethod> EngagementMethod { get; set; }

    public DbSet<TutorTeachingInformation> TutorTeachingInformation { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Message>()
             .HasOne(x => x.Recipient)
             .WithMany(x => x.MessagesReceived)
             .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
            .HasOne(x => x.Sender)
            .WithMany(x => x.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
    }


}