using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        [DefaultValue(false)]
        public bool IsComplete { get; set; }
    }
}
