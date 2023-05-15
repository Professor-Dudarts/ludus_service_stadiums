using LudusStadium.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace LudusStadium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly LudusStadiumsContext _context;

        private async Task<bool> isAvaliable(schedule newSchedule)
        {
            var scheduleStadiumDate = await _context.schedules.Where(sc => sc.FK_Stadium_ID == newSchedule.FK_Stadium_ID && sc.matchDate.Date == newSchedule.matchDate.Date).ToListAsync();

            if (scheduleStadiumDate.Count == 1)
            {
                foreach (var item in scheduleStadiumDate)
                {
                    return newSchedule.matchDate > item.matchDate.Add(TimeSpan.FromHours(3)) ? true : false;
                }
            }

            return scheduleStadiumDate.Count == 2 ? false : true;
        }

        public ScheduleController(LudusStadiumsContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var schedule = await _context.schedules.ToListAsync();
            return Ok(schedule);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduleById(int id)
        {
            var schedule = await _context.schedules.FindAsync(id);
            if (schedule == null)
            {
                ModelState.AddModelError("Schedule", "Agendamento não encontrado");
                return BadRequest(ModelState);
            }

            return Ok(schedule);
        }

        [HttpGet("ByDateTime/{dateTime}")]
        public async Task<IActionResult> GetScheduleByDate(DateTime dateTime)
        {
            var schedule = await _context.schedules.Where(sc => sc.matchDate == dateTime).ToListAsync();
            if (schedule == null || schedule.Count == 0)
            {
                ModelState.AddModelError("Schedule", "Agendamento não encontrado");
                return BadRequest(ModelState);
            }

            return Ok(schedule);
        }

        [HttpGet("ByStadiumName/{name}")]
        public async Task<IActionResult> GetScheduleByStadiumName(string name)
        {
            var stadium = await _context.stadia.Where(st => st.name == name).Include(st => st.Address).FirstOrDefaultAsync(st => st.ID == st.ID);

            if (stadium == null)
            {
                ModelState.AddModelError("Stadium", "Estadio não encontrado");
                return NotFound(ModelState);
            }

            var schedule = await _context.schedules.Where(sc => sc.FK_Stadium_ID == stadium.ID).ToListAsync();
            if (schedule == null || schedule.Count == 0)
            {
                ModelState.AddModelError("Schedule", "Agendamento não encontrado");
                return NotFound(ModelState);
            }

            return Ok(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] schedule newSchedule)
        {
            var stadium = await _context.stadia.Include(st => st.Address).FirstOrDefaultAsync(st => st.ID == newSchedule.FK_Stadium_ID);

            if (stadium == null)
            {
                ModelState.AddModelError("Stadium", "Estadio não encontrado");
                return NotFound(ModelState);
            }

            if (await isAvaliable(newSchedule))
            {                
                await _context.schedules.AddAsync(newSchedule);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetScheduleById), new { id = newSchedule.ID }, newSchedule);
            }
            else
            {
                ModelState.AddModelError("Stadium", "Estadio não disponivel");
                return NotFound(ModelState);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStadium(int id, [FromBody] schedule updatedSchedule)
        {
            var schedule = await _context.schedules.FindAsync(id);

            if (schedule == null)
            {
                return NotFound();
            }

            if (await isAvaliable(updatedSchedule))
            {
                schedule.matchDate = updatedSchedule.matchDate;
                schedule.FK_Match_ID = updatedSchedule.FK_Match_ID;
                schedule.FK_Stadium_ID = updatedSchedule.FK_Stadium_ID;

                await _context.SaveChangesAsync();

                return Ok(schedule);
            }
            else
            {
                ModelState.AddModelError("Stadium", "Estadio não disponivel");
                return NotFound(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStadium(int id)
        {
            var schedule = await _context.schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            _context.schedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}