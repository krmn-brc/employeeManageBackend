using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class DepartmentRepository : IGenericRepository<Department>
    {
        private readonly AppDbContext _appDbContext;

        public DepartmentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<GeneralResponse> DeleteByIdAsync(int id)
        {
            var dep = await _appDbContext.Departments.FindAsync(id);
            if(dep == null)
                return NotFound();
            _appDbContext.Departments.Remove(dep);
            await Commit();
            return Success();
        }

        public async Task<ICollection<Department>> GetAllAsync()
        => await _appDbContext.Departments.AsNoTracking().Include(x => x.GeneralDepartment).ToListAsync();


        public async Task<Department?> GetByIdAsync(int id)
        => await _appDbContext.Departments.FindAsync(id);

        public async Task<GeneralResponse> InsertAsync(Department item)
        {
            if(!await CheckName(item.Name!)) 
                return new GeneralResponse(false, "Department already added");
            _appDbContext.Departments.Add(item);
            await Commit();
            return Success();
        }


        public async Task<GeneralResponse> UpdateAsync(Department item)
        {
            var dep = await _appDbContext.Departments.FindAsync(item.Id);
            if(dep == null)
                return NotFound();
            dep.Name = item.Name;
            dep.GeneralDepartmentId = item.GeneralDepartmentId;
            await Commit();
            return Success();
        }
        private async Task<bool> CheckName(string name)
        {
           var item = await _appDbContext.Departments.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
           return item is null;
        }

        public static GeneralResponse NotFound() => new(false, "Sorry department not found.");
        public static GeneralResponse Success() => new(true, "Process completed."); 
        private async Task Commit() => await _appDbContext.SaveChangesAsync();
    }
}