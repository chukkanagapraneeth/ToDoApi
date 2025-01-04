using Microsoft.AspNetCore.Mvc;
using ToDoApi.DTOs;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        public static List<string> _todos = new List<string>(); 

        [HttpGet()]
        public IActionResult GetTodos()
        {
            return Ok(_todos);
        }

        [HttpPost()]
        public IActionResult CreateTodo([FromBody] string msg)
        {
            _todos.Add(msg);
            return Ok("Added");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTodo(int id, [FromBody] string msg)
        {
            _todos[id] = msg;
            return Ok(_todos);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(int id)
        {
            _todos.RemoveAt(id);
            return Ok("Removed");
        }
    }
}
