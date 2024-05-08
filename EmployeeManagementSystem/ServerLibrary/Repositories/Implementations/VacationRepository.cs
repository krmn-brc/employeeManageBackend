
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class VacationRepository : IGenericRepository<Vacation>
    {
        private readonly AppDbContext _appDbContext;

        public VacationRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<GeneralResponse> DeleteByIdAsync(int id)
        {
           var findVacation = await _appDbContext.Vacations.FirstOrDefaultAsync(x => x.EmployeeId == id);
           if (findVacation == null) return NotFound();
           _appDbContext.Vacations.Remove(findVacation);
           await Commit();
           return Success();
        }

        public async Task<ICollection<Vacation>> GetAllAsync()
        =>await _appDbContext.Vacations.AsNoTracking().Include(x => x.VacationType).ToListAsync();

        public async Task<Vacation?> GetByIdAsync(int id)
        => await _appDbContext.Vacations.AsNoTracking().Include(x => x.VacationType).FirstOrDefaultAsync(x => x.EmployeeId == id);

        public async Task<GeneralResponse> InsertAsync(Vacation item)
        {
            await _appDbContext.Vacations.AddAsync(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> UpdateAsync(Vacation item)
        {
            var findVacation = await _appDbContext.Vacations.FirstOrDefaultAsync(x => x.EmployeeId == item.EmployeeId);
            if (findVacation == null) return NotFound();
            findVacation.StartDate = item.StartDate;
            findVacation.NumberOfDays = item.NumberOfDays;
            findVacation.VacationTypeId = item.VacationTypeId;
            await Commit();
            return Success();
        }
        private async Task Commit() => await _appDbContext.SaveChangesAsync();
        private GeneralResponse NotFound() => new (false, "Sorry employee not found");
        private GeneralResponse Success() => new(true, "Process completed");
    }
}