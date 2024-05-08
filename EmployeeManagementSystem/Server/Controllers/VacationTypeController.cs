using BaseLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VacationTypeController : GenericController<VacationType>
    {
        public VacationTypeController(IGenericRepository<VacationType> genericRepository) : base(genericRepository)
        {
        }
    }


}