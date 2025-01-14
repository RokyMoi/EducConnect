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

    public DbSet<PersonPhoneNumber> PersonPhoneNumber { get; set; }
    public DbSet<Tutor> Tutor { get; set; }

    public DbSet<TutorRegistrationStatus> TutorRegistrationStatus { get; set; }
    public DbSet<Student> Student { get; set; }
    public DbSet<StudentDetails> StudentDetails { get; set; }

    public DbSet<Country> Country { get; set; }

    public DbSet<LearningCategory> LearningCategory { get; set; }
    public DbSet<EduConnect.Entities.Learning.LearningSubcategory> LearningSubCategory { get; set; }

    public DbSet<PersonEducationInformation> PersonEducationInformation { get; set; }

    public DbSet<EmploymentType> EmploymentType { get; set; }
    public DbSet<WorkType> WorkType { get; set; }

    public DbSet<TutorTeachingStyleType> TutorTeachingStyleType { get; set; }
    public DbSet<PersonCareerInformation> PersonCareerInformation { get; set; }

    public DbSet<PersonAvailability> PersonAvailibility { get; set; }

    public DbSet<CommunicationType> CommunicationType { get; set; }

    public DbSet<EngagementMethod> EngagementMethod { get; set; }

    public DbSet<TutorTeachingInformation> TutorTeachingInformation { get; set; }

    public DbSet<IndustryClassification> IndustryClassification { get; set; }

    public DbSet<Course> Course { get; set; }

    public DbSet<CourseDetails> CourseDetails { get; set; }


    public DbSet<LearningDifficultyLevel> LearningDifficultyLevel { get; set; }

    public DbSet<Language> Language { get; set; }

    public DbSet<CourseType> CourseType { get; set; }

    public DbSet<CourseLanguage> CourseLanguage { get; set; }

    public DbSet<CourseCreationCompletenessStep> CourseCreationCompletenessStep { get; set; }

    public DbSet<CourseMainMaterial> CourseMainMaterial { get; set; }

    public DbSet<CourseLesson> CourseLesson { get; set; }

    public DbSet<CourseLessonSupplementaryMaterial> CourseLessonSupplementaryMaterial { get; set; }

    public DbSet<CourseLessonContent> CourseLessonContent { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseLanguage>()
            .HasKey(cl => new { cl.CourseId, cl.LanguageId });


    }



}