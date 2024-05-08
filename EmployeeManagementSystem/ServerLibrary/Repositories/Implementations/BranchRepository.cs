using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class BranchRepository : IGenericRepository<Branch>
    {
        private readonly AppDbContext _appDbContext;

        public BranchRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<GeneralResponse> DeleteByIdAsync(int id)
        {
            var dep = await _appDbContext.Branches.FindAsync(id);
            if(dep == null)
                return NotFound();
            _appDbContext.Branches.Remove(dep);
            await Commit();
            return Success();
        }

        public async Task<ICollection<Branch>> GetAllAsync()
        => await _appDbContext
        .Branches
        .AsNoTracking()
        .Include(x => x.Department)
        .ToListAsync();


        public async Task<Branch?> GetByIdAsync(int id)
        => await _appDbContext.Branches.FindAsync(id);

        public async Task<GeneralResponse> InsertAsync(Branch item)
        {
            if(!await CheckName(item.Name!)) 
                return new GeneralResponse(false, "Branch already added");
            _appDbContext.Branches.Add(item);
            await Commit();
            return Success();
        }


        public async Task<GeneralResponse> UpdateAsync(Branch item)
        {
            var dep = await _appDbContext.Branches.FindAsync(item.Id);
            if(dep == null)
                return NotFound();
            dep.Name = item.Name;
            dep.DepartmentId = item.DepartmentId;
            await Commit();
            return Success();
        }
        private async Task<bool> CheckName(string name)
        {
           var item = await _appDbContext.Branches.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
           return item is null;
        }

        public static GeneralResponse NotFound() => new(false, "Sorry Branch not found.");
        public static GeneralResponse Success() => new(true, "Process completed."); 
        private async Task Commit() => await _appDbContext.SaveChangesAsync();
    }
}