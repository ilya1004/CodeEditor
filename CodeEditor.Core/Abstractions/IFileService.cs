namespace CodeEditor.Core.Abstractions;

public interface IFileService
{
    string ReadFile(string filePath);
    void SaveFile(string filePath, string content);
}