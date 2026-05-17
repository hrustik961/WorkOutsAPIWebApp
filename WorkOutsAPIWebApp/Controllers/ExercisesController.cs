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
    public class ExercisesController : ControllerBase
    {
        private readonly WorkOutsAPIContext _context;

        public ExercisesController(WorkOutsAPIContext context)
        {
            _context = context;
        }

        // GET: api/Exercises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Exercises>>> GetExercises()
        {
            return await _context.Exercises.ToListAsync();
        }

        // GET: api/Exercises/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Exercises>> GetExercises(int id)
        {
            var exercises = await _context.Exercises.FindAsync(id);

            if (exercises == null)
            {
                return NotFound();
            }

            return exercises;
        }

        // PUT: api/Exercises/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExercises(int id, Exercises exercises)
        {
            if (id != exercises.Id)
            {
                return BadRequest();
            }

            _context.Entry(exercises).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExercisesExists(id))
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

        // POST: api/Exercises
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // POST: api/Exercises
        [HttpPost]
        public async Task<ActionResult<Exercises>> PostExercises(Exercises exercises)
        {
            var trackedWorkOuts = new List<WorkOuts>();

            if (exercises.WorkOuts != null)
            {
                foreach (var workout in exercises.WorkOuts)
                {
                    var existingWorkout = await _context.WorkOuts.FindAsync(workout.Id);
                    if (existingWorkout != null)
                    {
                        trackedWorkOuts.Add(existingWorkout);
                    }
                }
            }

            exercises.WorkOuts = trackedWorkOuts;

            _context.Exercises.Add(exercises);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExercises", new { id = exercises.Id }, exercises);
        }

        // DELETE: api/Exercises/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercises(int id)
        {
            var exercises = await _context.Exercises.FindAsync(id);
            if (exercises == null)
            {
                return NotFound();
            }

            _context.Exercises.Remove(exercises);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExercisesExists(int id)
        {
            return _context.Exercises.Any(e => e.Id == id);
        }
    }
}
