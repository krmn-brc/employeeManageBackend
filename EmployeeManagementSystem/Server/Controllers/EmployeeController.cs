using BaseLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : GenericController<Employee>
    {
        public EmployeeController(IGenericRepository<Employee> genericRepository) : base(genericRepository)
        {
        }
    }
}