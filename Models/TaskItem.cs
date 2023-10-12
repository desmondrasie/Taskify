using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taskify.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public bool IsChecked { get; set; } = false;
        public TaskList? TaskList { get; set; } 
        public int TaskListId { get; set; }
        public DueDetails DueDetails { get; set; } = new();
    }
}   
