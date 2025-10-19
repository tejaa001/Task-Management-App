// Importing necessary libraries for the controller to work
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

// Defining the namespace for this controller
namespace TodoApi.Controllers
{
    // Setting up the route for this controller and marking it as an API controller
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        // Creating a private field to hold our database context
        private readonly TodoContext _context;

        // Constructor that receives a database context when the controller is created
        public TodosController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Todos - This method handles getting all todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            // Return all todos from the database as a list
            return await _context.Todos.ToListAsync();
        }

        // GET: api/Todos/5 - This method handles getting a specific todo by its ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            // Find the todo with the specified ID in the database
            var todo = await _context.Todos.FindAsync(id);

            // If no todo was found, return a 404 Not Found response
            if (todo == null)
            {
                return NotFound();
            }

            // If todo was found, return it
            return todo;
        }

        // POST: api/Todos - This method handles creating a new todo
        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo(Todo todo)
        {
            // Add the new todo to the database
            _context.Todos.Add(todo);
            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Return a 201 Created response with the new todo and its location
            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        // PUT: api/Todos/5 - This method handles updating an existing todo
        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> PutTodo(int id, Todo todo)
        {
            // Check if the ID in the URL matches the ID in the request body
            if (id != todo.Id)
            {
                // If they don't match, return a 400 Bad Request response
                return BadRequest(new { message = "ID in URL and request body must match" });
            }

            // Mark the todo as modified in the database context
            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                // Try to save the changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // If the todo doesn't exist, return a 404 Not Found response
                if (!TodoExists(id))
                {
                    return NotFound(new { message = $"Todo with ID {id} not found" });
                }
                else
                {
                    // If there's another issue, re-throw the exception
                    throw;
                }
            }

            // Return the updated todo with a 200 OK response
            return Ok(todo);
        }

        // DELETE: api/Todos/5 - This method handles deleting a todo
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteTodo(int id)
        {
            // Find the todo with the specified ID
            var todo = await _context.Todos.FindAsync(id);
            // If no todo was found, return a 404 Not Found response
            if (todo == null)
            {
                return NotFound(new { message = $"Todo with ID {id} not found" });
            }

            // Remove the todo from the database context
            _context.Todos.Remove(todo);
            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Return a success message with the deleted todo's ID
            return Ok(new
            {
                message = $"Todo with ID {id} deleted successfully",
                deletedId = id
            });
        }
        
        // Helper method to check if a todo with the specified ID exists
        private bool TodoExists(int id)
        {
            // Returns true if any todo in the database has the specified ID
            return _context.Todos.Any(e => e.Id == id);
        }
    }
}