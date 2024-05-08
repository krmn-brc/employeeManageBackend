using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericController<T> : ControllerBase
    where T : class
    {
        private readonly IGenericRepository<T> _genericRepository;

        public GenericController(IGenericRepository<T> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll() => Ok(await _genericRepository.GetAllAsync());

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if(id <= 0) return BadRequest("Invalid request sent");
            return Ok(await _genericRepository.DeleteByIdAsync(id));
        }

        [HttpGet("single/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if(id <= 0) return BadRequest("Invalid request sent");
            return Ok(await _genericRepository.GetByIdAsync(id));
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(T model)
        {
            if(model is null) return BadRequest("Bad request made");
            return Ok(await _genericRepository.InsertAsync(model));
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update(T model)
        {
            if(model is null) return BadRequest("Bad request made");
            return Ok(await _genericRepository.UpdateAsync(model));
        }
    }
}