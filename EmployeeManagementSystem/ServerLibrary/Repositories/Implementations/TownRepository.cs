using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class TownRepository : IGenericRepository<Town>
    {
        private readonly AppDbContext _appDbContext;

        public TownRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<GeneralResponse> DeleteByIdAsync(int id)
        {
            var dep = await _appDbContext.Towns.FindAsync(id);
            if(dep == null)
                return NotFound();
            _appDbContext.Towns.Remove(dep);
            await Commit();
            return Success();
        }

        public async Task<ICollection<Town>> GetAllAsync()
        => await _appDbContext
        .Towns
        .AsNoTracking()
        .Include(x => x.City)
        .ToListAsync();


        public async Task<Town?> GetByIdAsync(int id)
        => await _appDbContext.Towns.FindAsync(id);

        public async Task<GeneralResponse> InsertAsync(Town item)
        {
            if(!await CheckName(item.Name!)) 
                return new GeneralResponse(false, "Town already added");
            _appDbContext.Towns.Add(item);
            await Commit();
            return Success();
        }


        public async Task<GeneralResponse> UpdateAsync(Town item)
        {
            var dep = await _appDbContext.Towns.FindAsync(item.Id);
            if(dep == null)
                return NotFound();
            dep.Name = item.Name;
            dep.CityId = item.CityId;
            await Commit();
            return Success();
        }
        private async Task<bool> CheckName(string name)
        {
           var item = await _appDbContext.Towns.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
           return item is null;
        }

        public static GeneralResponse NotFound() => new(false, "Sorry Town not found.");
        public static GeneralResponse Success() => new(true, "Process completed."); 
        private async Task Commit() => await _appDbContext.SaveChangesAsync();
    }
}