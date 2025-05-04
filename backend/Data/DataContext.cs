using backend.Entities.Learning;
using backend.Entities.Person;
using backend.Entities.Reference;
using backend.Entities.Reference.Country;
using EduConnect.Entities.Tutor;
using EduConnect.Entities.Person;
using EduConnect.Entities.Reference;
using EduConnect.Entities.Student;
using Microsoft.EntityFrameworkCore;
using EduConnect.Entities.Course;
using backend.Entities.Reference.Learning;
using backend.Entities.Reference.Language;
using backend.Entities.Course;
using EduConnect.Entities.Messenger;
using EduConnect.Entities.Shopping;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EduConnect.Data;

public class DataContext : IdentityDbContext<Person, IdentityRole<Guid>, Guid>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Person> Person { get; set; }
    public DbSet<PersonDetails> PersonDetails { get; set; }
    public DbSet<PersonEmail> PersonEmail { get; set; }
    public DbSet<PersonPassword> PersonPassword { get; set; }
    public DbSet<PersonProfilePicture> PersonProfilePicture { get; set; }
    public DbSet<PersonSalt> PersonSalt { get; set; }
    public DbSet<PersonPhoto> PersonPhoto { get; set; }
    public DbSet<PersonVerificationCode> PersonVerificationCode { get; set; }
    public DbSet<PersonPhoneNumber> PersonPhoneNumber { get; set; }
    public DbSet<AuthenticationToken> AuthenticationToken { get; set; }
    public DbSet<Tutor> Tutor { get; set; }
    public DbSet<TutorRegistrationStatus> TutorRegistrationStatus { get; set; }
    public DbSet<Student> Student { get; set; }
    public DbSet<Message> Message { get; set; }

    public DbSet<StudentDetails> StudentDetails { get; set; }
    public DbSet<Country> Country { get; set; }
    public DbSet<Course> Course { get; set; }
 //SHOPPING PARTA
    public DbSet<ShoppingCart> ShoppingCart { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    public DbSet<Wishlist> Wishlist { get; set; }
    public DbSet<WishlistItems> WishlistItems { get; set; }
    public DbSet<StudentEnrollment> StudentEnrollment { get; set; }




    public DbSet<LearningCategory> LearningCategory { get; set; }
    public DbSet<EduConnect.Entities.Learning.LearningSubcategory> LearningSubCategory { get; set; }
    public DbSet<PersonEducationInformation> PersonEducationInformation { get; set; }
    public DbSet<EmploymentType> EmploymentType { get; set; }
    public DbSet<WorkType> WorkType { get; set; }
    public DbSet<TutorTeachingStyleType> TutorTeachingStyleType { get; set; }
    public DbSet<PersonCareerInformation> PersonCareerInformation { get; set; }
    public DbSet<PersonAvailability> PersonAvailibility { get; set; }
    public DbSet<CommunicationType> CommunicationType { get; set; }
    public DbSet<IndustryClassification> IndustryClassification { get; set; }
    public DbSet<EngagementMethod> EngagementMethod { get; set; }
    public DbSet<TutorTeachingInformation> TutorTeachingInformation { get; set; }
    public DbSet<CourseDetails> CourseDetails { get; set; }
    public DbSet<LearningDifficultyLevel> LearningDifficultyLevel { get; set; }
    public DbSet<Language> Language { get; set; }
    public DbSet<CourseCategory> CourseCategory { get; set; }
    public DbSet<CourseLanguage> CourseLanguage { get; set; }
    public DbSet<Tag> Tag { get; set; }
    public DbSet<CourseTag> CourseTag { get; set; }
    public DbSet<CourseThumbnail> CourseThumbnail { get; set; }
    public DbSet<CourseTeachingResource> CourseTeachingResource { get; set; }

    public DbSet<CourseLesson> CourseLesson { get; set; }

    public DbSet<CourseLessonContent> CourseLessonContent { get; set; }

    

    public DbSet<CourseLessonResource> CourseLessonResource { get; set; }

    public DbSet<CoursePromotionImage> CoursePromotionImage { get; set; }

    public DbSet<CourseViewershipData> CourseViewershipData { get; set; }

    public DbSet<CourseViewershipDataSnapshot> CourseViewershipDataSnapshot { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Person>().ToTable("Person", "Person");

        builder.Entity<Person>(
            b =>
            {
                b.ToTable("Person", "Person");
                b.HasKey(x => x.PersonId);
                b.Property(x => x.PersonId).HasColumnName("PersonId");
                b.Property(x => x.CreatedAt).HasColumnName("CreatedAt");
            }
        );

        builder.Entity<Person>().Ignore(x => x.Id)
            .Ignore(x => x.PasswordHash)
            .Ignore(x => x.Email)
            .Ignore(x => x.PhoneNumber)
            .Ignore(x => x.UserName);

        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLoginLog");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");

        builder.Entity<CourseTag>()
            .HasOne(ct => ct.Course)
            .WithMany()
            .HasForeignKey(ct => ct.CourseId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.Entity<CourseTag>()
            .HasOne(ct => ct.Tag)
            .WithMany()
            .HasForeignKey(ct => ct.TagId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(p => p.MessagesSent)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
            .HasOne(m => m.Recipient)
            .WithMany(p => p.MessagesReceived)
            .HasForeignKey(m => m.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationship between Course and CourseCategory
        builder.Entity<Course>()
            .HasOne(c => c.CourseCategory)
            .WithMany()
            .HasForeignKey(c => c.CourseCategoryId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        // Configure the relationship between Course and Tutor
        builder.Entity<Course>()
            .HasOne(c => c.Tutor)
            .WithMany()
            .HasForeignKey(c => c.TutorId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        // Configure the relationship between Course and LearningDifficultyLevel
        builder.Entity<Course>()
            .HasOne(c => c.LearningDifficultyLevel)
            .WithMany()
            .HasForeignKey(c => c.LearningDifficultyLevelId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}