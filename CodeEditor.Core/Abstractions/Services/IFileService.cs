namespace CodeEditor.Core.Abstractions.Services;

public interface IFileService
{
    string ReadFile(string filePath);
    void SaveFile(string filePath, string content);
    void CreateFile(string path);
    void CreateDirectory(string path);
    void RenameFile(string oldPath, string newPath);
    void RenameDirectory(string oldPath, string newPath);
    void DeleteFile(string path);
    void DeleteDirectory(string path);
}