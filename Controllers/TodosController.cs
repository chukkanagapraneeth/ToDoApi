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
            var todo = _todos.FirstOrDefault(x => x.Id == id);
            if (todo == null)
            {
                return NotFound($"Todo with Id {id} not found. Please enter a valid id.");
            }
            return Ok(todo);
        }

        [HttpPost()]
        public IActionResult CreateTodo([FromBody] TodoDTO todoDTO)
        {
            Todo newTodo = new Todo();
            newTodo.Id = _todos.Count() + 1;
            newTodo.Title = todoDTO.Title;
            newTodo.Description = todoDTO.Description;
            newTodo.IsComplete = todoDTO.IsComplete;

            _todos.Add(newTodo);

            // We should return 201 when a resource is created

            return CreatedAtAction(nameof(GetTodo), new { id = newTodo.Id }, newTodo);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTodo(int id, [FromBody] TodoDTO todoDTO)
        {

            var todo = _todos.FirstOrDefault(x => x.Id == id);

            if(todo == null)
            {
                return NotFound($"Todo with Id {id} not found. Please enter a valid id.");
            }

            todo.Title = todoDTO.Title;
            todo.Description = todoDTO.Description;
            todo.IsComplete = todoDTO.IsComplete;
            return Ok(todo);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(int id)
        {
            var todo = _todos.FirstOrDefault(x => x.Id == id);

            if(todo == null)
            {
                return NotFound($"Todo with Id {id} not found. Please enter a valid id.");
            }

            _todos.Remove(todo);

            return Ok($"Todo with id {id} Removed Successfully.");
        }
    }
}
