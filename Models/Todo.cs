using System.ComponentModel;

namespace ToDoApi.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        [DefaultValue(false)]
        public bool IsComplete { get; set; }
    }
}
