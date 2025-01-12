using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;

namespace ToDoApi.Data
{
    public class TodosDataContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }

        public TodosDataContext(DbContextOptions<TodosDataContext> options) : base(options)
        {
            
        }
    }
}
