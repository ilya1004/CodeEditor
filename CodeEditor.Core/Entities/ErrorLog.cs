namespace CodeEditor.Core.Entities;

public class ErrorLog : Entity
{
    public DateTime Timestamp { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
}