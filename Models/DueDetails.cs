using System.ComponentModel.DataAnnotations;
namespace ThoughtHarbour.Models

{
    public class DueDetails
    {
        [Key]
        public int Id { get; set; }
        public DateTime? DueDate { get; set; } 
        public TimeSpan? DueTime { get; set; }
        public int TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; } = null!;
    }
}
