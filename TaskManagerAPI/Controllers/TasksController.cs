using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        // GET Daily
        [HttpGet("daily")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetDailyTasks()
        {
            return await _context.Tasks.Where(t => t.Frequency == TaskFrequency.Daily).ToListAsync();
        }

        // GET Haftalık 
        [HttpGet("weekly")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetWeeklyTasks()
        {
            return await _context.Tasks.Where(t => t.Frequency == TaskFrequency.Weekly).ToListAsync();
        }

        // GET Aylık
        [HttpGet("monthly")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetMonthlyTasks()
        {
            return await _context.Tasks.Where(t => t.Frequency == TaskFrequency.Monthly).ToListAsync();
        }

        // ADD
        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTask(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
        }

        // Update
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, TaskItem task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
