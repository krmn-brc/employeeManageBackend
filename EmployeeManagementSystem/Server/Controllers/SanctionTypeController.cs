using BaseLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SanctionTypeController : GenericController<SanctionType>
    {
        public SanctionTypeController(IGenericRepository<SanctionType> genericRepository) : base(genericRepository)
        {
        }
    }


}