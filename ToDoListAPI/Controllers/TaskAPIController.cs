using Microsoft.AspNetCore.Mvc;
using ToDoListAPI.Data;
using ToDoListAPI.Model;
using ToDoListAPI.Model.Dto;

namespace ToDoListAPI.Controllers
{
    [Route("api/TaskAPI")]
    [ApiController]
    public class TaskAPIController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public TaskAPIController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ToDoTask>> GetTasks()
        {
            return Ok(db.Tasks.OrderBy(t=>t.Id));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ToDoTask> CreateTask([FromBody] ToDoTaskDTO task)
        {
            if (task == null || string.IsNullOrEmpty(task.Name) || string.IsNullOrWhiteSpace(task.Description))
            {
                return BadRequest("Invalid data");
            }
            ToDoTask model = new ToDoTask()
            {
                Name = task.Name,
                Description = task.Description,
                DueDate = task.DueDate
            };

            db.Tasks.Add(model);
            db.SaveChanges();

            return Ok(model);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteTask(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid id");
            }
            var task = db.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound("Task not found");
            }
            db.Tasks.Remove(task);
            db.SaveChanges();
            return Ok("Success");
        }

        [HttpPut("Complete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ToDoTask> CompleteTask(int id)
        {
            if(id <= 0)
            {
                return BadRequest("Invalid id");
            }
            var task = db.Tasks.FirstOrDefault(t => t.Id == id);
            if(task == null)
            {
                return NotFound("Task not found");
            }
            task.Status = "Completed";
            db.SaveChanges();
            return Ok(task);
        }
    }
}
