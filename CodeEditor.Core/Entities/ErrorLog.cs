namespace CodeEditor.Core.Entities;

public class ErrorLog : Entity
{
    public DateTime Timestamp { get; init; }
    public string Message { get; init; }
    public string? StackTrace { get; init; }
}