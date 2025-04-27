using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CodeEditor.Core.Abstractions;
using CodeEditor.Core.Commands;
using CodeEditor.Core.Models;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;

namespace CodeEditor.Core.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly IFileService _fileService;
    public FileExplorerViewModel FileExplorerVM { get; init; }
    public ICommand OpenFileCommand { get; }
    public ICommand SaveFileCommand { get; }
    public ICommand RunCodeCommand { get; }

    public ObservableCollection<string> Languages { get; } = ["Python", "Perl", "C#"];

    private readonly Dictionary<string, string> _extensionToLanguage = new()
    {
        { ".py", "Python" },
        { ".pl", "Perl" },
        { ".cs", "C#" },
        { ".csproj", "C#" }
    };

    private string _selectedFilePath = string.Empty;
    public string SelectedFilePath
    {
        get => _selectedFilePath;
        set
        {
            _selectedFilePath = value;
            OnPropertyChanged();
            UpdateSyntaxHighlighting();
            UpdateLaunchParameters();
        }
    }

    private TextDocument _codeDocument = new();
    public TextDocument CodeDocument
    {
        get => _codeDocument;
        set
        {
            _codeDocument = value;
            OnPropertyChanged();
        }
    }

    private IHighlightingDefinition? _syntaxHighlighting;
    public IHighlightingDefinition? SyntaxHighlighting
    {
        get => _syntaxHighlighting;
        set
        {
            _syntaxHighlighting = value;
            OnPropertyChanged();
        }
    }

    private string? _selectedLanguage;
    public string? SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            _selectedLanguage = value;
            OnPropertyChanged();
            UpdateLaunchParameters();
            UpdateSyntaxHighlighting();
        }
    }

    private string _launchParameters = string.Empty;
    public string LaunchParameters
    {
        get => _launchParameters;
        set
        {
            _launchParameters = value;
            OnPropertyChanged();
        }
    }

    public MainWindowViewModel(IFileService fileService, FileExplorerViewModel fileExplorerViewModel)
    {
        _fileService = fileService;
        FileExplorerVM = fileExplorerViewModel;
        SaveFileCommand = new RelayCommand(SaveFile);
        OpenFileCommand = new RelayParamsCommand<FileSystemItem>(OpenFile!);
        RunCodeCommand = new RelayCommand(RunCode, CanRunCode);
    }
    
    private void OpenFile(FileSystemItem fileSystemItem)
    {
        SelectedFilePath = fileSystemItem.FullPath;
        var content = _fileService.ReadFile(fileSystemItem.FullPath);
        CodeDocument.Text = content;
        var extension = Path.GetExtension(fileSystemItem.FullPath).ToLower();
        SelectedLanguage = _extensionToLanguage.GetValueOrDefault(extension);

        UpdateSyntaxHighlighting();
    }
    
    private void SaveFile()
    {
        if (!string.IsNullOrEmpty(SelectedFilePath))
        {
            _fileService.SaveFile(SelectedFilePath, CodeDocument.Text);
        }
    }

    private bool CanSaveFile()
    {
        return !string.IsNullOrEmpty(SelectedFilePath) && File.Exists(SelectedFilePath);
    }

    private void UpdateLaunchParameters()
    {
        if (string.IsNullOrEmpty(SelectedFilePath))
        {
            LaunchParameters = string.Empty;
            return;
        }

        LaunchParameters = SelectedLanguage switch
        {
            "Python" => $"python \"{SelectedFilePath}\"",
            "Perl" => $"perl \"{Path.GetFileName(SelectedFilePath)}\"",
            "C#" => GetCSharpLaunchCommand(),
            _ => string.Empty
        };
    }

    private string GetCSharpLaunchCommand()
    {
        var extension = Path.GetExtension(SelectedFilePath).ToLower();
       
        if (extension == ".csproj")
        {
            return $"dotnet run --project \"{SelectedFilePath}\"";
        }

        if (extension == ".cs")
        {
            var directory = Path.GetDirectoryName(SelectedFilePath);
            if (!string.IsNullOrEmpty(directory))
            {
                var csprojFiles = Directory.GetFiles(directory, "*.csproj");
                if (csprojFiles.Length > 0)
                {
                    return $"dotnet run --project \"{csprojFiles[0]}\"";
                }
            }
        }
        return $"dotnet run \"{SelectedFilePath}\"";
    }

    private void UpdateSyntaxHighlighting()
    {
        if (string.IsNullOrEmpty(SelectedFilePath))
        {
            SyntaxHighlighting = null;
            return;
        }

        var extension = Path.GetExtension(SelectedFilePath).ToLower();
        SyntaxHighlighting = SelectedLanguage switch
        {
            "Python" => HighlightingManager.Instance.GetDefinition("Python"),
            "Perl" => HighlightingManager.Instance.GetDefinition("Perl"),
            "C#" => HighlightingManager.Instance.GetDefinition("C#"),
            _ => HighlightingManager.Instance.GetDefinitionByExtension(extension)
        };
    }

    private void RunCode()
    {
        if (string.IsNullOrEmpty(SelectedFilePath) || string.IsNullOrEmpty(SelectedLanguage))
            return;

        string command = SelectedLanguage switch
        {
            "Perl" => $"cd \"{Path.GetDirectoryName(SelectedFilePath) ?? ""}\" && chcp 65001 && {LaunchParameters} & pause",
            _ => $"{LaunchParameters} & pause"
        };

        Process.Start(new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/k {command}",
            UseShellExecute = true
        });
    }
    
    private bool CanRunCode()
    {
        return !string.IsNullOrEmpty(SelectedFilePath) && !string.IsNullOrEmpty(SelectedLanguage) && File.Exists(SelectedFilePath);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}