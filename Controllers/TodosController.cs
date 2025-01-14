using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public TodosController(TodosDataContext context)
        {
            _todoContext = context;
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpGet()]
        public IActionResult GetTodos()
        {
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
        public IActionResult GetTodo(int id)
        {
            var todo = _todoContext.Todos.Find(id);
            if (todo == null)
            {
                return NotFound($"Todo with Id {id} not found. Please enter a valid id.");
            }
            return Ok(todo);
        }

        [HttpPost, Authorize]
        public IActionResult CreateTodo([FromBody] TodoDTO todoDTO)
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;

            Todo newTodo = new Todo();
            newTodo.Title = todoDTO.Title;
            newTodo.Description = todoDTO.Description;
            newTodo.IsComplete = todoDTO.IsComplete;
            newTodo.UserId = userId ?? "User0"; 

            _todoContext.Todos.Add(newTodo);

            _todoContext.SaveChanges();

            // We should return 201 when a resource is created

            return CreatedAtAction(nameof(GetTodo), new { id = newTodo.Id }, newTodo);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateTodo(int id, [FromBody] TodoDTO todoDTO)
        {

            var todo = _todoContext.Todos.Find(id);

            if(todo == null)
            {
                return NotFound($"Todo with Id {id} not found. Please enter a valid id.");
            }

            todo.Title = todoDTO.Title;
            todo.Description = todoDTO.Description;
            todo.IsComplete = todoDTO.IsComplete;

            _todoContext.SaveChanges();
            
            return Ok(todo);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteTodo(int id)
        {
            var todo = _todoContext.Todos.Find(id);

            if(todo == null)
            {
                return NotFound($"Todo with Id {id} not found. Please enter a valid id.");
            }

            _todoContext.Todos.Remove(todo);
            _todoContext.SaveChanges();

            return Ok($"Todo with id {id} Removed Successfully.");
        }
    }
}
