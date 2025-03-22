using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CodeEditor.Core.Abstractions;
using CodeEditor.Core.Commands;
using CodeEditor.Core.Models;
using Microsoft.Win32;
// ReSharper disable InconsistentNaming

namespace CodeEditor.Core.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly IFileService _fileService;
    public FileExplorerViewModel FileExplorerVM { get; init; }
    public ICommand OpenFileCommand { get; }
    public ICommand SaveFileCommand { get; }

    private string _selectedFilePath = string.Empty;

    public string SelectedFilePath
    {
        get => _selectedFilePath;
        set
        {
            _selectedFilePath = value;
            OnPropertyChanged();
        }
    }

    private string _codeText = string.Empty;

    public string CodeText
    {
        get => _codeText;
        set
        {
            if (string.IsNullOrEmpty(SelectedFilePath))
            {
                return;
            }
            _codeText = value;
            OnPropertyChanged();
        }
    }
    public MainWindowViewModel(IFileService fileService, FileExplorerViewModel fileExplorerViewModel)
    {
        _fileService = fileService;
        FileExplorerVM = fileExplorerViewModel;
        SaveFileCommand = new RelayCommand(SaveFile);
        OpenFileCommand = new RelayParamsCommand<FileSystemItem>(OpenFile!);
    }
    
    private void OpenFile(FileSystemItem fileSystemItem)
    {
        SelectedFilePath = fileSystemItem.FullPath;
        CodeText = _fileService.ReadFile(fileSystemItem.FullPath);
    }
    
    private void SaveFile()
    {
        _fileService.SaveFile(SelectedFilePath, CodeText);
    }

    private bool CanSaveFile()
    {
        return !string.IsNullOrEmpty(SelectedFilePath) && Path.Exists(SelectedFilePath);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}