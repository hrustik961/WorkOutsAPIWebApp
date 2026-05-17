using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkOutsAPIWebApp.Models;

namespace WorkOutsAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOutsController : ControllerBase
    {
        private readonly WorkOutsAPIContext _context;

        public WorkOutsController(WorkOutsAPIContext context)
        {
            _context = context;
        }

        // GET: api/WorkOuts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkOuts>>> GetWorkOuts()
        {
            return await _context.WorkOuts.ToListAsync();
        }

        // GET: api/WorkOuts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkOuts>> GetWorkOuts(int id)
        {
            var workOuts = await _context.WorkOuts.FindAsync(id);

            if (workOuts == null)
            {
                return NotFound();
            }

            return workOuts;
        }

        // PUT: api/WorkOuts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkOuts(int id, WorkOuts workOuts)
        {
            if (id != workOuts.Id)
            {
                return BadRequest();
            }

            _context.Entry(workOuts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkOutsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WorkOuts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkOuts>> PostWorkOuts(WorkOuts workOuts)
        {
            _context.WorkOuts.Add(workOuts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkOuts", new { id = workOuts.Id }, workOuts);
        }

        // DELETE: api/WorkOuts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkOuts(int id)
        {
            var workOuts = await _context.WorkOuts
                .Include(w => w.Exercises)
                .Include(w => w.Categories)
                .FirstOrDefaultAsync(w => w.Id == id);
            if (workOuts == null)
            {
                return NotFound();
            }

            _context.WorkOuts.Remove(workOuts);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkOutsExists(int id)
        {
            return _context.WorkOuts.Any(e => e.Id == id);
        }
    }
}
