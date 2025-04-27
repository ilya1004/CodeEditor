namespace CodeEditor.Core.Abstractions.Services;

public interface IDialogService
{
    bool? ShowInputDialog(string prompt, out string result);
}