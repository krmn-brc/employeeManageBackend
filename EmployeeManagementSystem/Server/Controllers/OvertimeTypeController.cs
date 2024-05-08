using BaseLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OvertimeTypeController : GenericController<OvertimeType>
    {
        public OvertimeTypeController(IGenericRepository<OvertimeType> genericRepository) : base(genericRepository)
        {
        }
    }


}