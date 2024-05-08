using BaseLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SanctionController : GenericController<Sanction>
    {
        public SanctionController(IGenericRepository<Sanction> genericRepository) : base(genericRepository)
        {
        }
    }


}