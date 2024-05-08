using BaseLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TownController : GenericController<Town>
    {
        public TownController(IGenericRepository<Town> genericRepository) : base(genericRepository)
        {
        }
    }
}