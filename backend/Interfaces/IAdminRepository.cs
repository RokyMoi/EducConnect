using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.DTOs;

namespace EduConnect.Interfaces
{
    public interface IAdminRepository
    {
        Task<bool> SeedData();
        Task<bool> DataExists();

        Task<List<GetAllUsersResponse>> GetAllUsers();
    }
}