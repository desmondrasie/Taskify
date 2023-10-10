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
        public TaskList TaskList { get; set; } = null!;
        public int TaskListId { get; set; }
        public TaskDueDetails? DueDetails { get; set; }


    }
}   
