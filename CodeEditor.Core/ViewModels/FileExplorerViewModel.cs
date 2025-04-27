using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CodeEditor.Core.Commands;
using CodeEditor.Core.Models;
using Microsoft.Win32;

namespace CodeEditor.Core.ViewModels;

public class FileExplorerViewModel : INotifyPropertyChanged 
{ 
    public ObservableCollection<FileSystemItem> Items { get; set; } = [];
    public ICommand SelectFolderCommand { get; set; }
    public ICommand OpenItemCommand { get; set; }
    public ICommand GoUpCommand { get; set; }
    public ICommand GoBackCommand { get; set; }

    private readonly Stack<string> _navigationHistory = new();
    
    private string _currentPath = "Select folder";
    private string CurrentPath
    {
        get => _currentPath;
        set
        {
            _currentPath = value;
            _navigationHistory.Push(_currentPath);
            OnPropertyChanged();
        }
    }
    
    public FileExplorerViewModel()
    {
        GoUpCommand = new RelayCommand(GoUp, CanGoUp);
        GoBackCommand = new RelayCommand(GoBack, CanGoBack);
        SelectFolderCommand = new RelayCommand(SelectFolder);
        OpenItemCommand = new RelayParamsCommand<FileSystemItem>(OpenItem!);
    }

    private void SelectFolder()
    {
        var dialog = new OpenFolderDialog();
        if (dialog.ShowDialog() == true)
        {
            _navigationHistory.Clear();
            LoadFolder(dialog.FolderName);
        }
    }

    private void LoadFolder(string path)
    {
        if (!Directory.Exists(path)) return;

        Items.Clear();
        CurrentPath = path;

        try
        {
            foreach (var dir in Directory.GetDirectories(path))
            {
                Items.Add(new FileSystemItem
                {
                    Name = Path.GetFileName(dir),
                    FullPath = dir,
                    IsDirectory = true,
                    Icon = "📁"
                });
            }

            foreach (var file in Directory.GetFiles(path))
            {
                Items.Add(new FileSystemItem
                {
                    Name = Path.GetFileName(file),
                    FullPath = file,
                    IsDirectory = false,
                    Icon = "📄"
                });
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Loading folder failed", ex);
        }
    }

    private void OpenItem(FileSystemItem item)
    {
        if (item.IsDirectory)
        {
            LoadFolder(item.FullPath);
        }
    }
    
    private void GoBack()
    {
        if (_navigationHistory.Count > 1)
        {
            _navigationHistory.Pop();
            var previousPath = _navigationHistory.Pop();
            LoadFolder(previousPath);
        }
    }

    private bool CanGoBack() => _navigationHistory.Count > 0;

    private void GoUp()
    {
        var newPath = CurrentPath[..CurrentPath.LastIndexOf('\\')];
        if (newPath.Last() == ':')
        {
            newPath += '\\';
        }
        
        if (!Path.Exists(newPath))
        {
            throw new Exception("This path does not exist");
        }  
        
        LoadFolder(newPath);
    }

    private bool CanGoUp()
    {
        if (CurrentPath.Contains('\\'))
        {
            var newPath = CurrentPath[..CurrentPath.LastIndexOf('\\')];
            if (newPath.Last() == ':')
            {
                newPath += '\\';
            }
            if (Path.Exists(newPath))
            {
                return true;
            }   
        }

        return false;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }  
}