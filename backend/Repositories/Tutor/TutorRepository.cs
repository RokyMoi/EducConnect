using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Tutor;
using backend.Interfaces.Tutor;
using EduConnect.Data;

namespace backend.Repositories.Tutor
{
    public class TutorRepository : ITutorRepository
    {
        private readonly DataContext _databaseContext;

        public TutorRepository(DataContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public async Task<TutorDTO> CreateTutor(EduConnect.Entities.Tutor.Tutor tutor)
        {
            await _databaseContext.Tutor.AddAsync(tutor);
            await _databaseContext.SaveChangesAsync();
            return new TutorDTO
            {
                TutorId = tutor.TutorId,
                PersonId = tutor.PersonId
            };

        }
    }
}