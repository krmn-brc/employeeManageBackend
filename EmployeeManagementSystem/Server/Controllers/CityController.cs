using BaseLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : GenericController<City>
    {
        public CityController(IGenericRepository<City> genericRepository) : base(genericRepository)
        {
        }
    }
}