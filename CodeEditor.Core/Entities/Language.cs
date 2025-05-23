namespace CodeEditor.Core.Entities;

public class Language : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Extensions { get; set; } = string.Empty; // Список расширений, разделённых запятыми, например: ".cs,.csproj"
    public string RunCommand { get; set; } = string.Empty; // Команда запуска, например: "node \"{0}\""
    public bool IsExecutable { get; set; } // Можно ли выполнять код
    public string DocumentationUrl { get; set; } = string.Empty; // Ссылка на документацию
}