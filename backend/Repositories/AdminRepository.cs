using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Data;
using EduConnect.DTOs;
using EduConnect.Entities.Course;
using EduConnect.Entities.Person;
using EduConnect.Interfaces;
using EduConnect.Utilities;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Repositories
{
    public class AdminRepository(DataContext _dataContext, PersonManager _personManager) : IAdminRepository
    {
        public async Task<bool> DataExists()
        {
            return await _dataContext.Course.AnyAsync();
        }

        public async Task<List<GetAllUsersResponse>> GetAllUsers()
        {
            return await
            _dataContext.Person
            .Include(x => x.PersonEmail)
            .Include(x => x.PersonDetails)
            .Include(x => x.UserRoles)
            .Select(
                x => new GetAllUsersResponse
                {
                    PersonId = x.PersonId,
                    FirstName = x.PersonDetails.FirstName,
                    LastName = x.PersonDetails.LastName,
                    Email = x.PersonEmail.Email,
                    Username = x.PersonDetails.Username,
                    Role = _dataContext.Roles
                        .Where(y => x.UserRoles.FirstOrDefault() != null && x.UserRoles.FirstOrDefault().RoleId.Equals(y.Id))
                        .Select(y => y.Name)
                        .FirstOrDefault() ?? "Unknown",
                    PersonPublicId = x.PersonPublicId,

                }
            ).ToListAsync();
        }

        public async Task<bool> SeedData()
        {
            //Get reference data
            var countries = await _dataContext.Country.ToListAsync();
            var difficultyLevels = await _dataContext.LearningDifficultyLevel.Select(x => x.LearningDifficultyLevelId).ToListAsync();

            var random = new Random();


            var addedPersons = new List<Person>();

            for (int i = 0; i < 50; i++)
            {
                var firstName = Faker.Name.First();
                var lastName = Faker.Name.Last();
                var firseNameLower = firstName.ToLower();
                var lastNameLower = lastName.ToLower();
                var username = $"{firseNameLower}.{lastNameLower}";
                var email = $"{firseNameLower}.{lastNameLower}@educonnect.com";
                var password = "12345678";
                var hashedPassword = EncryptionUtilities.HashPassword(password);

                var person = new Person
                {
                    PersonId = Guid.NewGuid(),
                    PersonPublicId = Guid.NewGuid(),
                    IsActive = false,
                    UserName = username,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    ModifiedAt = null
                };

                var personEmail = new EduConnect.Entities.Person.PersonEmail
                {
                    PersonEmailId = Guid.NewGuid(),
                    Email = $"{username}@educonnect.com",
                    PersonId = person.PersonId,
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    ModifiedAt = null
                };

                var personPassword = new EduConnect.Entities.Person.PersonPassword
                {
                    PersonPasswordId = Guid.NewGuid(),
                    PersonId = person.PersonId,
                    PasswordHash = hashedPassword,
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    ModifiedAt = null
                };

                var personDetails = new EduConnect.Entities.Person.PersonDetails
                {
                    PersonDetailsId = Guid.NewGuid(),
                    PersonId = person.PersonId,
                    FirstName = firstName,
                    LastName = lastName,
                    Username = username,
                    CountryOfOriginCountryId = countries[random.Next(countries.Count)].CountryId,
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),


                };

                await _dataContext.Person.AddAsync(person);
                await _dataContext.PersonEmail.AddAsync(personEmail);

                await _dataContext.PersonPassword.AddAsync(personPassword);
                await _dataContext.PersonDetails.AddAsync(personDetails);

                // if (i < 25)
                // {
                //     await _personManager.AddToRoleAsync(person, "Tutor");
                //     var tutor = new EduConnect.Entities.Tutor.Tutor
                //     {
                //         TutorId = Guid.NewGuid(),
                //         PersonId = person.PersonId,
                //         TutorRegistrationStatusId = 1,
                //         CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                //         ModifiedAt = null

                //     };

                //     await _dataContext.Tutor.AddAsync(tutor);
                // }

                // if (i > 24)
                // {
                //     await _personManager.AddToRoleAsync(person, "Student");
                //     var student = new EduConnect.Entities.Student.Student
                //     {
                //         StudentId = Guid.NewGuid(),
                //         PersonId = person.PersonId,
                //         CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                //         ModifiedAt = null
                //     };
                // }

                // await _dataContext.SaveChangesAsync();
                addedPersons.Add(person);

            }

            await _dataContext.SaveChangesAsync();
            Console.WriteLine($"Data seeded successfully. Number of persons added: {addedPersons.Count}");

            var tutors = new List<EduConnect.Entities.Tutor.Tutor>();
            var students = new List<EduConnect.Entities.Student.Student>();

            for (int i = 0; i < 50; i++)
            {
                if (i > 24)
                {
                    await _personManager.AddToRoleAsync(addedPersons[i], "Student");
                    var student = new EduConnect.Entities.Student.Student
                    {
                        StudentId = Guid.NewGuid(),
                        PersonId = addedPersons[i].PersonId,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        ModifiedAt = null
                    };
                    await _dataContext.Student.AddAsync(student);
                    students.Add(student);
                }
                if (i < 25)
                {
                    await _personManager.AddToRoleAsync(addedPersons[i], "Tutor");
                    var tutor = new EduConnect.Entities.Tutor.Tutor
                    {
                        TutorId = Guid.NewGuid(),
                        PersonId = addedPersons[i].PersonId,
                        TutorRegistrationStatusId = 1,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        ModifiedAt = null

                    };

                    await _dataContext.Tutor.AddAsync(tutor);
                    tutors.Add(tutor);
                }
            }

            await _dataContext.SaveChangesAsync();

            var categories = new List<CourseCategory>();
            for (int i = 0; i < 10; i++)
            {
                var category = new CourseCategory
                {
                    CourseCategoryId = Guid.NewGuid(),
                    Name = $"Category {i + 1}",
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    UpdatedAt = null
                };

                await _dataContext.CourseCategory.AddAsync(category);
                categories.Add(category);
            }
            await _dataContext.SaveChangesAsync();

            var courses = new List<EduConnect.Entities.Course.Course>();

            for (int i = 0; i < 100; i++)
            {
                int? minNumberOfStudents = random.Next(0, 10);
                if (minNumberOfStudents == 0)
                {
                    minNumberOfStudents = null;
                }

                int? maxNumberOfStudents = minNumberOfStudents != null ? random.Next(minNumberOfStudents.Value, 100) : random.Next(50, 100);

                if (maxNumberOfStudents == 51)
                {
                    maxNumberOfStudents = null;
                }
                var course = new EduConnect.Entities.Course.Course
                {
                    CourseId = Guid.NewGuid(),
                    Title = $"Sample Course {i + 1}",
                    Description = $"Description for Course {i + 1}, this is description extender for course {i + 1}",
                    Price = random.Next(0, 100),
                    CourseCategoryId = categories[random.Next(categories.Count)].CourseCategoryId,
                    TutorId = tutors[random.Next(tutors.Count)].TutorId,
                    LearningDifficultyLevelId = random.Next(difficultyLevels.Min(), difficultyLevels.Max()),
                    MinNumberOfStudents = minNumberOfStudents,
                    MaxNumberOfStudents = maxNumberOfStudents,
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    UpdatedAt = null,
                };
                await _dataContext.Course.AddAsync(course);
                courses.Add(course);
            }
            await _dataContext.SaveChangesAsync();

            for (int i = 0; i < courses.Count; i++)
            {
                var courseResourceList = new List<CourseTeachingResource>()
                {
                    new CourseTeachingResource
                    {
                        CourseId = courses[i].CourseId,
                        Title = $"Sample Resource 1 for ${courses[i].Title}",
                        Description = $"Description for ${courses[i].Title} with additional information, this is description extender for course {i + 1}",
                        ResourceUrl = Faker.Internet.Url(),
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        UpdatedAt = null
                    },
                    new CourseTeachingResource
                    {
                        CourseId = courses[i].CourseId,
                        Title = $"Sample Resource 2 for ${courses[i].Title}",
                        Description = $"Description for ${courses[i].Title} with additional information, this is description extender for course {i + 1}",
                        ResourceUrl = Faker.Internet.Url(),
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        UpdatedAt = null
                    },
                    new CourseTeachingResource
                    {
                        CourseId = courses[i].CourseId,
                        Title = $"Sample Resource 3 for ${courses[i].Title}",
                        Description = $"Description for ${courses[i].Title} with additional information, this is description extender for course {i + 1}",
                        ResourceUrl = Faker.Internet.Url(),
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        UpdatedAt = null
                    },
                    new CourseTeachingResource
                    {
                        CourseId = courses[i].CourseId,
                        Title = $"Sample Resource 4 for ${courses[i].Title}",
                        Description = $"Description for ${courses[i].Title} with additional information, this is description extender for course {i + 1}",
                        ResourceUrl = Faker.Internet.Url(),
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        UpdatedAt = null
                    },


                };

                await _dataContext.CourseTeachingResource.AddRangeAsync(courseResourceList);

            }

            await _dataContext.SaveChangesAsync();

            return true;




        }
    }
}