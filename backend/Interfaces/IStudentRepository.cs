using backend.DTOs.Student;
using EduConnect.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduConnect.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<StudentDTO>> GetAllStudents();
        Task<StudentDTO> GetStudentInfoByEmail(string email);

<<<<<<< HEAD
=======
        Task<StudentEntityDTO> GetStudentByPersonId(Guid personId);
>>>>>>> e62e459ed1d7fa44c20fc57eae494a1e84df3398
    }

}
