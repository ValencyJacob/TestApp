using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp.Models;
using TestApp.Services;

namespace TestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly OrganizationService _db;

        public OrganizationController(OrganizationService db)
        {
            _db = db;
        }

        /// <summary>
        /// GetAll
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>GetAll</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organization>>> GetAll()
        {
            var items = await _db.GetOrganizations();

            return Ok(items);
        }

        /// <summary>
        /// Get by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Get object by Id</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var obj = await _db.GetOrganization(id);

            if (obj == null)
                return NotFound();

            return Ok(obj);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Create</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Organization obj)
        {
            await _db.Create(obj);

            return Ok(obj);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns>Update</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Organization item)
        {
            var obj = await _db.GetOrganization(id);

            if (obj == null)
                return NotFound();

            if (obj.Id == item.Id)
            {
                await _db.Update(item);

                return Ok();
            }

            return NotFound();
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Delete</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _db.Remove(id);

            return Ok();
        }
    }
}
