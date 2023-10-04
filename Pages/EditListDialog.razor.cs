using MudBlazor;
using Microsoft.AspNetCore.Components;

namespace Taskify.Pages
{
    public partial class EditListDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;
        [Parameter] public string CurrentName { get; set; } = null!;

        private void Cancel() => MudDialog.Cancel();
        private void Save() => MudDialog.Close(DialogResult.Ok(CurrentName));
    }
}
