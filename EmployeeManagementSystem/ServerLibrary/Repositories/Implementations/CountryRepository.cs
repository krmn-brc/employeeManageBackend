using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class CountryRepository : IGenericRepository<Country>
    {
        private readonly  AppDbContext _appDbContext;

        public CountryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<GeneralResponse> DeleteByIdAsync(int id)
        {
            var dep = await _appDbContext.Countries.FindAsync(id);
            if(dep == null)
                return NotFound();
            _appDbContext.Countries.Remove(dep);
            await Commit();
            return Success();
        }

        public async Task<ICollection<Country>> GetAllAsync()
        => await _appDbContext.Countries.ToListAsync();


        public async Task<Country?> GetByIdAsync(int id)
        => await _appDbContext.Countries.FindAsync(id);

        public async Task<GeneralResponse> InsertAsync(Country item)
        {
            var d = await CheckName(item.Name!);

            var k = 0;
            if(!await CheckName(item.Name!)) 
                return new GeneralResponse(false, "Country already added");
            _appDbContext.Countries.Add(item);
            await Commit();
            return Success();
        }


        public async Task<GeneralResponse> UpdateAsync(Country item)
        {
            var dep = await _appDbContext.Countries.FindAsync(item.Id);
            if(dep == null)
                return NotFound();

            if(!await CheckName(item.Name!))
                return new GeneralResponse(false, "Country already added");
            
            dep.Name = item.Name;
            await Commit();
            return Success();
        }
        private async Task<bool> CheckName(string name)
        {
           var item = await _appDbContext.Countries.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
           return item is null;
        }

        public static GeneralResponse NotFound() => new(false, "Sorry Country not found.");
        public static GeneralResponse Success() => new(true, "Process completed."); 
        private async Task Commit() => await _appDbContext.SaveChangesAsync();
    }
}