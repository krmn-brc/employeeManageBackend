using BaseLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : GenericController<Country>
    {
        public CountryController(IGenericRepository<Country> genericRepository) : base(genericRepository)
        {
        }
    }
}