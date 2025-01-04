using Microsoft.AspNetCore.Mvc;
using ToDoApi.DTOs;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        public static List<Todo> _todos = new List<Todo>(); 

        [HttpGet()]
        public IActionResult GetTodos()
        {
            return Ok(_todos);
        }

        [HttpGet("{id}")]
        public IActionResult GetTodo(int id)
        {
            return Ok(_todos[id]);
        }

        [HttpPost()]
        public IActionResult CreateTodo([FromBody] TodoDTO todoDTO)
        {
            Todo t = new Todo();
            t.Id = _todos.Count() + 1;
            t.Title = todoDTO.Title;
            t.Description = todoDTO.Description;
            t.IsComplete = todoDTO.IsComplete;

            _todos.Add(t);
            return Ok("Added");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTodo(int id, [FromBody] TodoDTO todoDTO)
        {
            Todo t = new();
            t.Title = todoDTO.Title;
            t.Description = todoDTO.Description;
            t.IsComplete = todoDTO.IsComplete;

            _todos[id] = t;
            return Ok(_todos);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(int id)
        {

            for(int i = 0; i < _todos.Count; i++)
            {
                if( _todos[i].Id == id)
                {
                    _todos.RemoveAt(i);
                }
            }
            return Ok("Removed");
        }
    }
}
