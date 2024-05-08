
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class SanctionRepository : IGenericRepository<Sanction>
    {
        private readonly AppDbContext _appDbContext;

        public SanctionRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<GeneralResponse> DeleteByIdAsync(int id)
        {
            var findSanction = await _appDbContext.Sanctions.FirstOrDefaultAsync(s => s.EmployeeId == id);
            if (findSanction == null) return NotFound();
            _appDbContext.Sanctions.Remove(findSanction);
            await Commit();
            return Success();
        }

        public async Task<ICollection<Sanction>> GetAllAsync()
        => await _appDbContext.Sanctions.AsNoTracking().Include(x=> x.SanctionType).ToListAsync();

        public async Task<Sanction?> GetByIdAsync(int id)
        => await _appDbContext.Sanctions.AsNoTracking()
        .Include(x => x.SanctionType).FirstOrDefaultAsync(x => x.EmployeeId == id);

        public async Task<GeneralResponse> InsertAsync(Sanction item)
        {
            await _appDbContext.Sanctions.AddAsync(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> UpdateAsync(Sanction item)
        {
            var findSanction = await _appDbContext.Sanctions
            .Include(x => x.SanctionType).FirstOrDefaultAsync(x => x.EmployeeId == item.EmployeeId);
            if (findSanction == null) return NotFound();
            findSanction.PunishmentDate = item.PunishmentDate;
            findSanction.Punishment = item.Punishment;
            findSanction.Date = item.Date;
            findSanction.SanctionTypeId = item.SanctionTypeId;
            await Commit();
            return Success();
        }

        private async Task Commit() => await _appDbContext.SaveChangesAsync();
        private GeneralResponse NotFound() => new (false, "Sorry employee not found");
        private GeneralResponse Success() => new(true, "Process completed");
    }
}