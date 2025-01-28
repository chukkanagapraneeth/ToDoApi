using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Security.Claims;
using ToDoApi.Data;
using ToDoApi.DTOs;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        //public static List<Todo> _todos = new List<Todo>();
        private readonly TodosDataContext _todoContext;
        private readonly ILogger<TodosController> _logger;
        public TodosController(TodosDataContext context, ILogger<TodosController> logger)
        {
            _todoContext = context;
            _logger = logger;
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpGet()]
        public IActionResult GetTodos()
        {
            _logger.LogInformation("GetTodos Method initiated");
            var todos = _todoContext.Todos.ToList();
            return Ok(todos);
        }

        [HttpGet, Authorize]
        [Route("MyName")]
        public IActionResult GetMyName()
        {
            var userName = User.Identity?.Name;
            return Ok(new {userName});
        }

        [HttpGet, Authorize]
        [Route("MyTodos")]
        public IActionResult GetMyTodos()
        {
            var user = User.FindFirst(ClaimTypes.Name)?.Value;
            var myTodos = _todoContext.Todos.Where(x => x.UserId == user).ToList();

            return Ok(myTodos);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTodo(int id)
        {
            _logger.LogDebug($"{id}");
            var todo = await _todoContext.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound($"Todo with Id {id} not found. Please enter a valid id.");
            }
            return Ok(todo);
        }

        [HttpPost, Authorize]
        public IActionResult CreateTodo([FromBody] TodoDTO todoDTO)
        {
            _logger.LogDebug($"Todo {todoDTO}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;

            Todo newTodo = new Todo();
            newTodo.Title = todoDTO.Title;
            newTodo.Description = todoDTO.Description;
            newTodo.IsComplete = todoDTO.IsComplete;
            newTodo.UserId = userId ?? "User0";
            newTodo.CreatedAt = DateTime.Now;
            newTodo.LastModifiedAt = DateTime.Now;
            newTodo.RemindAt = todoDTO.RemindAt;

            _todoContext.Todos.Add(newTodo);

            _todoContext.SaveChanges();

            // We should return 201 when a resource is created

            return CreatedAtAction(nameof(GetTodo), new { id = newTodo.Id }, newTodo);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateTodo(int id, [FromBody] TodoDTO todoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = _todoContext.Todos.Find(id);
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;

            if(todo == null || todo.UserId != userId)
            {
                return Unauthorized($"You're not authorized to perform this action.");
            }

            todo.Title = todoDTO.Title;
            todo.Description = todoDTO.Description;
            todo.IsComplete = todoDTO.IsComplete;
            todo.RemindAt = todoDTO.RemindAt;
            todo.LastModifiedAt = DateTime.Now;

            _todoContext.SaveChanges();
            
            return Ok(todo);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteTodo(int id)
        {
            var todo = _todoContext.Todos.Find(id);
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;

            if(todo == null || todo.UserId != userId)
            {
                return Unauthorized($"You're not authorized to perform this action.");
            }

            _todoContext.Todos.Remove(todo);
            _todoContext.SaveChanges();

            return Ok($"Todo with id {id} Removed Successfully.");
        }
    }
}
