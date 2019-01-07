using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Friendster.Data;
using Friendster.Models;
using Microsoft.AspNetCore.Mvc;

namespace Friendster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IDataRepository _repo;

        public ValuesController(IDataRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Value>>> Get()
        {
            var values = await _repo.GetValuesAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Value>> Get(int id)
        {
            var value = await _repo.GetValueAsync(id);
            return Ok(value);
        }

    }
}
