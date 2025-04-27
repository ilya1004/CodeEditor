using CodeEditor.Core.Abstractions.Services;
using CodeEditor.Views.Views;

namespace CodeEditor.Views.Services;

public class DialogService : IDialogService
{
    public bool? ShowInputDialog(string prompt, out string result)
    {
        var dialog = new InputDialogWindow(prompt);
        var dialogResult = dialog.ShowDialog();
        result = dialogResult == true ? dialog.Result : string.Empty;
        return dialogResult;
    }
}