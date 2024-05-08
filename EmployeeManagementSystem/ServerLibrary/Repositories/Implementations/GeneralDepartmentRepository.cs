using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class GeneralDepartmentRepository : IGenericRepository<GeneralDepartment>
    {
        private readonly AppDbContext _appDbContext;

        public GeneralDepartmentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<GeneralResponse> DeleteByIdAsync(int id)
        {
            var dep = await _appDbContext.GeneralDepartments.FindAsync(id);
            if(dep == null)
                return NotFound();
            _appDbContext.GeneralDepartments.Remove(dep);
            await Commit();
            return Success();
        }

        public async Task<ICollection<GeneralDepartment>> GetAllAsync()
        => await _appDbContext.GeneralDepartments.ToListAsync();


        public async Task<GeneralDepartment?> GetByIdAsync(int id)
        => await _appDbContext.GeneralDepartments.FindAsync(id);

        public async Task<GeneralResponse> InsertAsync(GeneralDepartment item)
        {
            if(!await CheckName(item.Name!)) 
                return new GeneralResponse(false, "Department already added");
            _appDbContext.GeneralDepartments.Add(item);
            await Commit();
            return Success();
        }


        public async Task<GeneralResponse> UpdateAsync(GeneralDepartment item)
        {
            var dep = await _appDbContext.GeneralDepartments.FindAsync(item.Id);
            if(dep == null)
                return NotFound();
            dep.Name = item.Name;
            await Commit();
            return Success();
        }
        private async Task<bool> CheckName(string name)
        {
           var item = await _appDbContext.GeneralDepartments.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
           return item is null;
        }

        public static GeneralResponse NotFound() => new(false, "Sorry department not found.");
        public static GeneralResponse Success() => new(true, "Process completed."); 
        private async Task Commit() => await _appDbContext.SaveChangesAsync();
    }
}