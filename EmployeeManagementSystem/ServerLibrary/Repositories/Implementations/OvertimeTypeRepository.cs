using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class OvertimeTypeRepository : IGenericRepository<OvertimeType>
    {
        private readonly AppDbContext _appDbContext;

        public OvertimeTypeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<GeneralResponse> DeleteByIdAsync(int id)
        {
            var item = await _appDbContext.OvertimeTypes.FirstOrDefaultAsync(x=> x.Id == id);
            if (item == null) return NotFound();
            _appDbContext.OvertimeTypes.Remove(item);
            await Commit();
            return Success();
        }

        public async Task<ICollection<OvertimeType>> GetAllAsync()
        => await _appDbContext.OvertimeTypes.ToListAsync();

        public async Task<OvertimeType?> GetByIdAsync(int id)
        => await _appDbContext.OvertimeTypes.FindAsync(id);

        public async Task<GeneralResponse> InsertAsync(OvertimeType item)
        {
            if(!await CheckName(item.Name!)) 
                return new GeneralResponse(false, "Overtime Type already added");
            await _appDbContext.OvertimeTypes.AddAsync(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> UpdateAsync(OvertimeType item)
        {
            var obj = await _appDbContext.OvertimeTypes.FindAsync(item.Id);
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