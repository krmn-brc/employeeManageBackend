using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class EmployeeRepository : IGenericRepository<Employee>
    {
        private readonly AppDbContext _appDbContext;

        public EmployeeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<GeneralResponse> DeleteByIdAsync(int id)
        {
            var item = await _appDbContext.Employees.FindAsync(id);
            if (item == null) return NotFound();
            _appDbContext.Employees.Remove(item);
            await Commit();
            return Success();

        }
        
        public async Task<ICollection<Employee>> GetAllAsync()
        {
            var employees = await _appDbContext.Employees
            .AsNoTracking()
            .Include(t => t.Town)
            .ThenInclude(b => b.City)
            .ThenInclude(c => c.Country)
            .Include(b => b.Branch)
            .ThenInclude(d => d.Department)
            .ThenInclude(gd => gd.GeneralDepartment)
            .ToListAsync();
            return employees;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            var employee = await _appDbContext.Employees
            .AsNoTracking()
            .Include(t => t.Town)
            .ThenInclude(b => b.City)
            .ThenInclude(c => c.Country)
            .Include(b => b.Branch)
            .ThenInclude(d => d.Department)
            .ThenInclude(gd => gd.GeneralDepartment)
            .FirstOrDefaultAsync(x => x.Id == id);
            return employee!;
        }

        public async Task<GeneralResponse> InsertAsync(Employee item)
        {
            if(!await CheckName(item.Name!)) return new GeneralResponse(false, "Employee already added");
            _appDbContext.Employees.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> UpdateAsync(Employee item)
        {
            var findUser = await _appDbContext.Employees.FirstOrDefaultAsync(x => x.Id == item.Id);
            if(findUser == null) return NotFound();

            findUser.Name = item.Name;
            findUser.Other = item.Other;
            findUser.Address = item.Address;
            findUser.TelephoneNumber = item.TelephoneNumber;
            findUser.BranchId = item.BranchId;
            findUser.TownId = item.TownId;
            findUser.CivilId = item.CivilId;
            findUser.FileNumber = item.FileNumber;
            findUser.JobName = item.JobName;
            findUser.Photo = item.Photo;
            
            _appDbContext.Employees.Update(findUser);
            await Commit();
            return Success();
        }

        private async Task Commit() => await _appDbContext.SaveChangesAsync();
        private GeneralResponse NotFound() => new (false, "Sorry employee not found");
        private GeneralResponse Success() => new(true, "Process completed");
        private async Task<bool> CheckName(string name)
        {
            var item = await _appDbContext.Employees.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
            return item is null;
        }
    }
}