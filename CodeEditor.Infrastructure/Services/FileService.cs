using System.IO;

namespace CodeEditor.Infrastructure.Services;

public class FileService : IFileService
{
    public string ReadFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new ArgumentException("File path cannot be null or empty.");

        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found.", filePath);

        return File.ReadAllText(filePath);
    }

    public void SaveFile(string filePath, string content)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new ArgumentException("File path cannot be null or empty.");

        if (content == null)
            throw new ArgumentNullException(nameof(content), "Content cannot be null.");

        File.WriteAllText(filePath, content);
    }
}