using Ludus_Stadium.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ludus_Stadium.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StadiumController : ControllerBase
    {
        private readonly ludusstadiumsContext _context;

        public StadiumController(ludusstadiumsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var stadiums = await _context.stadia.ToListAsync();
            return Ok(stadiums);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var stadium = await _context.stadia.FindAsync(id);
            if (stadium == null) { return NotFound(); }

            return Ok(stadium);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] stadium newStadium)
        {
            await _context.stadia.AddAsync(newStadium);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = newStadium.id }, newStadium);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] stadium updatedStadium)
        {
            var stadium = await _context.stadia.FindAsync(id);
            if (stadium == null) { return NotFound(); }

            stadium.name = updatedStadium.name;
            stadium.adress = updatedStadium.adress;
            stadium.capacity = updatedStadium.capacity;
            stadium.openingDate = updatedStadium.openingDate;

            await _context.SaveChangesAsync();
            return Ok(stadium);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var stadium = await _context.stadia.FindAsync(id);
            if (stadium == null) { return NotFound(); }

            _context.stadia.Remove(stadium);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
