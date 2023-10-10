using System.ComponentModel.DataAnnotations;

namespace Taskify.Models
{
    public class TaskList
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
