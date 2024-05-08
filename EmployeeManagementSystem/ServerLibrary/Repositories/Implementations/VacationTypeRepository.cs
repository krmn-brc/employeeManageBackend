using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class VacationTypeRepository : IGenericRepository<VacationType>
    {
        private readonly AppDbContext _appDbContext;

        public VacationTypeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<GeneralResponse> DeleteByIdAsync(int id)
        {
            var item = await _appDbContext.VacationTypes.FindAsync(id);
            if (item == null) return NotFound();
            _appDbContext.VacationTypes.Remove(item);
            await Commit();
            return Success();
        }

        public async Task<ICollection<VacationType>> GetAllAsync()
        => await _appDbContext.VacationTypes.AsNoTracking().ToListAsync();

        public async Task<VacationType?> GetByIdAsync(int id)
        => await _appDbContext.VacationTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<GeneralResponse> InsertAsync(VacationType item)
        {
            if(!await CheckName(item.Name!))
                return new GeneralResponse(false, "Vacation Type allready added");
            await _appDbContext.VacationTypes.AddAsync(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> UpdateAsync(VacationType item)
        {
            var obj = await _appDbContext.VacationTypes.FindAsync(item.Id);
            if (obj == null) return NotFound();
            obj.Name = item.Name;
            await Commit();
            return Success();
        }

        private async Task<bool> CheckName(string name)
        {
            var item = await _appDbContext.OvertimeTypes.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
            return item is null;
        }

        private async Task Commit() => await _appDbContext.SaveChangesAsync();
        private GeneralResponse NotFound() => new (false, "Sorry employee not found");
        private GeneralResponse Success() => new(true, "Process completed");
    }
}