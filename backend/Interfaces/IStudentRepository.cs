using EduConnect.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduConnect.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<StudentDTO>> GetAllStudents(); 
        Task<StudentDTO> GetStudentInfoByEmail(string email);

    }

}
