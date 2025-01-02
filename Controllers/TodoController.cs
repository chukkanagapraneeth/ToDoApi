using Microsoft.AspNetCore.Mvc;
using ToDoApi.DTOs;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        public static List<string> _todos = new List<string>(); 

        [HttpGet("/GetTodos")]
        public IActionResult GetTodos()
        {
            return Ok(_todos);
        }

        [HttpPost("/CreateTodo")]
        public IActionResult CreateTodo([FromBody] string msg)
        {
            _todos.Add(msg);
            return Ok("Added");
        }

        [HttpPut("/UpdateTodo")]
        public IActionResult UpdateTodo([FromBody] UpdateDTO update)
        {
            _todos[update.Id] = update.msg;
            return Ok(_todos);
        }

        [HttpDelete("/DeleteTodo")]
        public IActionResult DeleteTodo(int id)
        {
            _todos.RemoveAt(id);
            return Ok("Removed");
        }
    }
}
