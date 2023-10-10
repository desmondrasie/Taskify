using System.ComponentModel.DataAnnotations;
namespace Taskify.Models

{
    public class TaskDueDetails
    {
        [Key]
        public int Id { get; set; }
        public DateTime? DueDate { get; set; }
        public TimeSpan? DueTime { get; set; }
        public bool IsOverdue
        {
            get
            {
                if (DueDate.HasValue)
                {
                    return DueDate.Value.Date < DateTime.Now.Date;
                }
                return false;
            }
        }
        public int TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; } = null!;

    }

}
