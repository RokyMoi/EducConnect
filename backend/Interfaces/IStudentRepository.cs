using EduConnect.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EduConnect.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<StudentDTO>> GetAllStudents(); 
        Task<StudentDTO> GetStudentInfoByEmail(string email);
    }

}
