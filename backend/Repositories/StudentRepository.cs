
using EduConnect.Data;
using EduConnect.DTOs;
using EduConnect.Extensions;
using EduConnect.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class StudentRepository : IStudentRepository
{
    private readonly DataContext _databaseContext;
   

    public StudentRepository(DataContext databaseContext)
    {
        _databaseContext = databaseContext;
       
    }

    public async Task<IEnumerable<StudentDTO>> GetAllStudents()
    {

        var students = await _databaseContext.Student
            .Include(s => s.Person)
                .ThenInclude(p => p.PersonDetails)
            .Include(s => s.StudentDetails)
            .Include(s => s.Person.PersonEmail)
            .ToListAsync();


        var studentDtos = students.Select(s => new StudentDTO
        {
            PersonId = s.PersonId,
            FirstName = s.Person.PersonDetails?.FirstName,
            LastName = s.Person.PersonDetails?.LastName,
            Username = s.Person.PersonDetails?.Username,
            Email = s.Person.PersonEmail?.Email,
            PhoneNumber = s.Person.PersonDetails?.PhoneNumber,
            CountryOfOrigin = s.Person.PersonDetails?.CountryOfOrigin,
            Biography = s.StudentDetails?.Biography,
            CurrentAcademicInstitution = s.StudentDetails?.CurrentAcademicInstitution,
            CurrentEducationLevel = s.StudentDetails?.CurrentEducationLevel,
            MainAreaOfSpecialization = s.StudentDetails?.MainAreaOfSpecialisation
        }).ToList();
        return studentDtos;
    }


    public async Task<StudentDTO> GetStudentInfoByEmail(string Email)
    {
       
        var student = await _databaseContext.Student
            .Where(s => s.Person.PersonEmail.Email == Email)
            .Include(s => s.Person)
                .ThenInclude(p => p.PersonDetails)
            .Include(s => s.StudentDetails)
            .Include(s => s.Person.PersonEmail)
            .FirstOrDefaultAsync();


        if (student == null)
        {
            return null;
        }

        else
        {
            var studentDto = new StudentDTO
            {
                PersonId = student.PersonId,
                FirstName = student.Person.PersonDetails?.FirstName,
                LastName = student.Person.PersonDetails?.LastName,
                Username = student.Person.PersonDetails?.Username,
                Email = student.Person.PersonEmail?.Email,
                PhoneNumber = student.Person.PersonDetails?.PhoneNumber,
                CountryOfOrigin = student.Person.PersonDetails?.CountryOfOrigin,
                Biography = student.StudentDetails?.Biography,
                CurrentAcademicInstitution = student.StudentDetails?.CurrentAcademicInstitution,
                CurrentEducationLevel = student.StudentDetails?.CurrentEducationLevel,
                MainAreaOfSpecialization = student.StudentDetails?.MainAreaOfSpecialisation
            };

            return studentDto;
        }
        
    }
    
}
