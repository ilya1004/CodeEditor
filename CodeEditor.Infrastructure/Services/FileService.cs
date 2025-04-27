using System.IO;
using CodeEditor.Core.Abstractions.Services;

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
    
    public void CreateFile(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException("Path cannot be null or empty.");

        File.Create(path).Dispose();
    }

    public void CreateDirectory(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException("Path cannot be null or empty.");

        Directory.CreateDirectory(path);
    }

    public void RenameFile(string oldPath, string newPath)
    {
        if (string.IsNullOrEmpty(oldPath) || string.IsNullOrEmpty(newPath))
            throw new ArgumentException("Paths cannot be null or empty.");

        if (!File.Exists(oldPath))
            throw new FileNotFoundException("File not found.", oldPath);

        File.Move(oldPath, newPath);
    }

    public void RenameDirectory(string oldPath, string newPath)
    {
        if (string.IsNullOrEmpty(oldPath) || string.IsNullOrEmpty(newPath))
            throw new ArgumentException("Paths cannot be null or empty.");

        if (!Directory.Exists(oldPath))
            throw new DirectoryNotFoundException("Directory not found.");

        Directory.Move(oldPath, newPath);
    }

    public void DeleteFile(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException("Path cannot be null or empty.");

        if (!File.Exists(path))
            throw new FileNotFoundException("File not found.", path);

        File.Delete(path);
    }

    public void DeleteDirectory(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException("Path cannot be null or empty.");

        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException("Directory not found.");

        Directory.Delete(path, true);
    }
}