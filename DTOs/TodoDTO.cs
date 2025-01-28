using System.ComponentModel.DataAnnotations;

namespace ToDoApi.DTOs
{
    public class TodoDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "Title Cannot exceed 100 characters")]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public bool IsComplete { get; set; }

        public DateTime? RemindAt { get; set; }
    }
}
