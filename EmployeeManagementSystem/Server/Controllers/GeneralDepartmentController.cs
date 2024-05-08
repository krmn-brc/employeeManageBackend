using BaseLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralDepartmentController : GenericController<GeneralDepartment>
    {
        public GeneralDepartmentController(IGenericRepository<GeneralDepartment> genericRepository) : base(genericRepository)
        {
        }
    }
}