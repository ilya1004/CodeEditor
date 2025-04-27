using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using CodeEditor.Core.Abstractions.Services;
using CodeEditor.Core.Commands;
using CodeEditor.Core.Models;
using Microsoft.Win32;

namespace CodeEditor.Core.ViewModels;

public class FileExplorerViewModel : INotifyPropertyChanged 
{ 
    private readonly IFileService _fileService;
    private readonly IDialogService _dialogService;
    public ObservableCollection<FileSystemItem> Items { get; set; } = [];
    public ICommand SelectFolderCommand { get; set; }
    public ICommand OpenItemCommand { get; set; }
    public ICommand GoUpCommand { get; set; }
    public ICommand GoBackCommand { get; set; }
    public ICommand CreateFileCommand { get; set; }
    public ICommand CreateFolderCommand { get; set; }
    public ICommand RenameItemCommand { get; set; }
    public ICommand DeleteItemCommand { get; set; }

    private readonly Stack<string> _navigationHistory = new();
    
    private string _currentPath = "Select folder";
    public string CurrentPath
    {
        get => _currentPath;
        set
        {
            _currentPath = value;
            _navigationHistory.Push(_currentPath);
            OnPropertyChanged();
        }
    }

    private FileSystemItem? _selectedItem;
    public FileSystemItem? SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged();
        }
    }
    
    public FileExplorerViewModel(IFileService fileService, IDialogService dialogService)
    {
        _fileService = fileService;
        _dialogService = dialogService;
        GoUpCommand = new RelayCommand(GoUp, CanGoUp);
        GoBackCommand = new RelayCommand(GoBack, CanGoBack);
        SelectFolderCommand = new RelayCommand(SelectFolder);
        OpenItemCommand = new RelayParamsCommand<FileSystemItem>(OpenItem!);
        CreateFileCommand = new RelayCommand(CreateFile, CanCreateFile);
        CreateFolderCommand = new RelayCommand(CreateFolder, CanCreateFile);
        RenameItemCommand = new RelayCommand(RenameItem, CanRenameOrDeleteItem);
        DeleteItemCommand = new RelayCommand(DeleteItem, CanRenameOrDeleteItem);
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

    private void CreateFile()
    {
        if (string.IsNullOrEmpty(CurrentPath) || CurrentPath == "Select folder") return;

        if (_dialogService.ShowInputDialog("Enter file name (e.g., example.cs):", out var fileName) == true && !string.IsNullOrWhiteSpace(fileName))
        {
            var fullPath = Path.Combine(CurrentPath, fileName.Trim());

            try
            {
                _fileService.CreateFile(fullPath); // Создаем пустой файл
                LoadFolder(CurrentPath); // Обновляем список файлов
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void CreateFolder()
    {
        if (string.IsNullOrEmpty(CurrentPath) || CurrentPath == "Select folder") return;

        if (_dialogService.ShowInputDialog("Enter folder name:", out var folderName) == true && !string.IsNullOrWhiteSpace(folderName))
        {
            var fullPath = Path.Combine(CurrentPath, folderName.Trim());

            try
            {
                _fileService.CreateDirectory(fullPath); // Создаем папку
                LoadFolder(CurrentPath); // Обновляем список файлов
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create folder: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private bool CanCreateFile()
    {
        return !string.IsNullOrEmpty(CurrentPath) && CurrentPath != "Select folder" && Directory.Exists(CurrentPath);
    }

    private void RenameItem()
    {
        if (SelectedItem == null) return;

        var prompt = SelectedItem.IsDirectory ? "Enter new folder name:" : "Enter new file name:";
        if (_dialogService.ShowInputDialog(prompt, out var newName) == true && !string.IsNullOrWhiteSpace(newName))
        {
            var newPath = Path.Combine(Path.GetDirectoryName(SelectedItem.FullPath) ?? CurrentPath, newName.Trim());

            try
            {
                if (SelectedItem.IsDirectory)
                {
                    _fileService.RenameDirectory(SelectedItem.FullPath, newPath);
                }
                else
                {
                    _fileService.RenameFile(SelectedItem.FullPath, newPath);
                }
                LoadFolder(CurrentPath); // Обновляем список
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to rename: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void DeleteItem()
    {
        if (SelectedItem == null) return;

        var message = SelectedItem.IsDirectory
            ? $"Are you sure you want to delete the folder '{SelectedItem.Name}' and all its contents?"
            : $"Are you sure you want to delete the file '{SelectedItem.Name}'?";
        var result = MessageBox.Show(message, "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                if (SelectedItem.IsDirectory)
                {
                    _fileService.DeleteDirectory(SelectedItem.FullPath);
                }
                else
                {
                    _fileService.DeleteFile(SelectedItem.FullPath);
                }
                LoadFolder(CurrentPath); // Обновляем список
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private bool CanRenameOrDeleteItem()
    {
        return SelectedItem != null && CanCreateFile();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }  
}