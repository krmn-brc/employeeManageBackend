using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class OvertimeRepository : IGenericRepository<Overtime>
    {
        private readonly AppDbContext _appDbContext;

        public OvertimeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<GeneralResponse> DeleteByIdAsync(int id)
        {
            var overtime = await _appDbContext.Overtimes.FirstOrDefaultAsync(o => o.EmployeeId == id);
            if (overtime == null) return NotFound();
            _appDbContext.Overtimes.Remove(overtime);
            await Commit();
            return Success();
        }

        public async Task<ICollection<Overtime>> GetAllAsync()
        => await _appDbContext.Overtimes
        .AsNoTracking()
        .Include(x => x.OvertimeType)
        .ToListAsync();

        public Task<Overtime?> GetByIdAsync(int id)
        => _appDbContext.Overtimes
        .AsNoTracking()
        .Include(x => x.OvertimeType)
        .FirstOrDefaultAsync(o => o.EmployeeId == id);

        public async Task<GeneralResponse> InsertAsync(Overtime item)
        {
            await _appDbContext.Overtimes.AddAsync(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> UpdateAsync(Overtime item)
        {
            var findOverTime = await _appDbContext.Overtimes.FirstOrDefaultAsync(o => o.EmployeeId == item.EmployeeId);
            if (findOverTime == null) return NotFound();
            findOverTime.EmployeeId = item.EmployeeId;
            findOverTime.StartDate = item.StartDate;
            findOverTime.EndDate = item.EndDate;
            findOverTime.OvertimeTypeId = item.OvertimeTypeId;

            await Commit();
            return Success();

        }

        public static GeneralResponse NotFound() => new(false, "Sorry department not found.");
        public static GeneralResponse Success() => new(true, "Process completed."); 
        private async Task Commit() => await _appDbContext.SaveChangesAsync();
    }
}