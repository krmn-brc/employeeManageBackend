using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class CityRepository : IGenericRepository<City>
    {
        private readonly AppDbContext _appDbContext;

        public CityRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<GeneralResponse> DeleteByIdAsync(int id)
        {
            var dep = await _appDbContext.Cities.FindAsync(id);
            if(dep == null)
                return NotFound();
            _appDbContext.Cities.Remove(dep);
            await Commit();
            return Success();
        }

        public async Task<ICollection<City>> GetAllAsync()
        => await _appDbContext
        .Cities
        .AsNoTracking()
        .Include(x => x.Country)
        .ToListAsync();


        public async Task<City?> GetByIdAsync(int id)
        => await _appDbContext.Cities.FindAsync(id);

        public async Task<GeneralResponse> InsertAsync(City item)
        {
            if(!await CheckName(item.Name!)) 
                return new GeneralResponse(false, "City already added");
            _appDbContext.Cities.Add(item);
            await Commit();
            return Success();
        }


        public async Task<GeneralResponse> UpdateAsync(City item)
        {
            var dep = await _appDbContext.Cities.FindAsync(item.Id);
            if(dep == null)
                return NotFound();
            dep.Name = item.Name;
            dep.CountryId = item.CountryId;
            await Commit();
            return Success();
        }
        private async Task<bool> CheckName(string name)
        {
           var item = await _appDbContext.Cities.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
           return item is null;
        }

        public static GeneralResponse NotFound() => new(false, "Sorry City not found.");
        public static GeneralResponse Success() => new(true, "Process completed."); 
        private async Task Commit() => await _appDbContext.SaveChangesAsync();
    }
}