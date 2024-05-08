using BaseLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VacationController : GenericController<Vacation>
    {
        public VacationController(IGenericRepository<Vacation> genericRepository) : base(genericRepository)
        {
        }
    }


}