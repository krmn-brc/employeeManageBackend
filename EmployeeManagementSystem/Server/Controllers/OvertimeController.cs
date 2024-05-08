using BaseLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OvertimeController : GenericController<Overtime>
    {
        public OvertimeController(IGenericRepository<Overtime> genericRepository) : base(genericRepository)
        {
        }
    }


}