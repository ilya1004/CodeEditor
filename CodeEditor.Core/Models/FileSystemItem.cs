using System.Collections.ObjectModel;

namespace CodeEditor.Core.Models;

public class FileSystemItem
{
    public string Name { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;
    public bool IsDirectory { get; set; }
    public string Icon { get; set; } = string.Empty; // Добавляем поле для иконки
}