using LudusStadium.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LudusStadium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StadiumController : ControllerBase
    {
        private readonly LudusStadiumsContext _context;

        private BadRequestObjectResult VerifyContent(stadium stadium)
        {
            if (stadium == null)
            {
                ModelState.AddModelError("Stadium", "Estádio é obrigatório.");
            }

            if (stadium.Address == null)
            {
                ModelState.AddModelError("Address", "Endereço é obrigatório.");
            }

            return BadRequest(ModelState);
        }

        public StadiumController(LudusStadiumsContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stadia = await _context.stadia.Include(st => st.Address).ToListAsync();
            return Ok(stadia);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStadium(int id)
        {
            var stadium = await _context.stadia.Include(st => st.Address).FirstOrDefaultAsync(st => st.ID == id);
            if (stadium == null)
            {
                return NotFound();
            }

            return Ok(stadium);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStadium([FromBody] stadium newStadium)
        {
            VerifyContent(newStadium);

            await _context.stadia.AddAsync(newStadium);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStadium), new { id = newStadium.ID }, newStadium);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStadium(int id, [FromBody] stadium updatedStadium)
        {
            var stadium = await _context.stadia.Include(st => st.Address).FirstOrDefaultAsync(st => st.ID == id);

            if (stadium == null)
            {
                return NotFound();
            }

            VerifyContent(updatedStadium);

            stadium.name = updatedStadium.name;
            stadium.capacity = updatedStadium.capacity;

            stadium.Address.street = updatedStadium.Address.street;
            stadium.Address.city = updatedStadium.Address.city;
            stadium.Address.state = updatedStadium.Address.state;
            stadium.Address.number = updatedStadium.Address.number;
            stadium.Address.zip = updatedStadium.Address.zip;

            await _context.SaveChangesAsync();

            return Ok(stadium);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStadium(int id)
        {
            var stadium = await _context.stadia.Include(st => st.Address).FirstOrDefaultAsync(st => st.ID == id);
            if (stadium == null)
            {
                return NotFound();
            }

            _context.stadia.Remove(stadium);
            _context.addresses.Remove(stadium.Address);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}