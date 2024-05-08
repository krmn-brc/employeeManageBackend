using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class DoctorRepository : IGenericRepository<Doctor>
    {
        private readonly AppDbContext _appDbContext;

        public DoctorRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<GeneralResponse> DeleteByIdAsync(int id)
        {
            var item = await _appDbContext.Doctors.FirstOrDefaultAsync(x => x.EmployeeId == id);
            if(item == null) return NotFound();
            _appDbContext.Doctors.Remove(item);
            await Commit();
            return Success();
        }

        public async Task<ICollection<Doctor>> GetAllAsync()
        => await _appDbContext.Doctors.AsNoTracking().ToListAsync();

        public async Task<Doctor?> GetByIdAsync(int id)
        => await _appDbContext.Doctors.AsNoTracking().FirstOrDefaultAsync(x => x.EmployeeId == id);
        public async Task<GeneralResponse> InsertAsync(Doctor item)
        {
            await _appDbContext.Doctors.AddAsync(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> UpdateAsync(Doctor item)
        {
            var findDoctor = await _appDbContext.Doctors.FirstOrDefaultAsync(x => x.EmployeeId == item.EmployeeId);
            if(findDoctor == null) return NotFound();
            findDoctor.EmployeeId = item.EmployeeId;
            findDoctor.Date = item.Date;
            findDoctor.MedicalDiagnose = item.MedicalDiagnose;
            findDoctor.MedicalRecommendation = item.MedicalRecommendation;
            // _appDbContext.Doctors.Update(findDoctor);
            await Commit();
            return Success();
        }
        public static GeneralResponse NotFound() => new(false, "Sorry department not found.");
        public static GeneralResponse Success() => new(true, "Process completed."); 
        private async Task Commit() => await _appDbContext.SaveChangesAsync();
    }
}