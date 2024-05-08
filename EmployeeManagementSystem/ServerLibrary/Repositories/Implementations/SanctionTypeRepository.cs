using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class SanctionTypeRepository : IGenericRepository<SanctionType>
    {
        private readonly AppDbContext _appDbContext;

        public SanctionTypeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<GeneralResponse> DeleteByIdAsync(int id)
        {
            var item = await _appDbContext.SanctionTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            _appDbContext.SanctionTypes.Remove(item);
            await Commit();
            return Success();
        }

        public async Task<ICollection<SanctionType>> GetAllAsync()
        => await _appDbContext.SanctionTypes.ToListAsync();

        public async Task<SanctionType?> GetByIdAsync(int id)
        => await _appDbContext.SanctionTypes.FindAsync(id);

        public async Task<GeneralResponse> InsertAsync(SanctionType item)
        {
            if(!await CheckName(item.Name!)) 
                return new GeneralResponse(false, "Sanction Type already added");
            await _appDbContext.SanctionTypes.AddAsync(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> UpdateAsync(SanctionType item)
        {
            var obj = await _appDbContext.SanctionTypes.FindAsync(item.Id);
            if(obj == null) return NotFound();
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