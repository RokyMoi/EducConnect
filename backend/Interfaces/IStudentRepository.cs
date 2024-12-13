using backend.DTOs.Student;
using EduConnect.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EduConnect.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<StudentDTO>> GetAllStudents();
        Task<StudentDTO> GetStudentInfoByEmail(string email);

        Task<StudentEntityDTO> GetStudentByPersonId(Guid personId);
    }

}
