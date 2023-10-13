using Microsoft.AspNetCore.Components;
using ThoughtHarbour.Models;

namespace ThoughtHarbour.Components
{
    public partial class TaskChunk
    {
        [Parameter] public TaskItem Task { get; set; } = null!;
        [Parameter] public EventCallback<TaskItem> OnDelete { get; set; }
        [Parameter] public EventCallback<TaskItem> OnEdit { get; set; }
        [Parameter] public EventCallback<TaskItem> OnStartEdit { get; set; }
        [Parameter] public EventCallback<bool> OnCheck { get; set; }

        public string noText = "<N/A>";

    }
}
