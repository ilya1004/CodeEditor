using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using CodeEditor.Core.Abstractions.Services;
using CodeEditor.Core.Commands;
using CodeEditor.Core.Models;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;

namespace CodeEditor.Core.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly IFileService _fileService;
    private readonly ILanguageService _languageService;
    public FileExplorerViewModel FileExplorerVM { get; init; }
    public ICommand OpenFileCommand { get; }
    public ICommand SaveFileCommand { get; }
    public ICommand RunCodeCommand { get; }

    public ObservableCollection<string> Languages { get; } = [];
    private readonly Dictionary<string, string> _extensionToLanguage = new();
    private readonly Dictionary<string, string> _languageToRunCommand = new();

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

    public MainWindowViewModel(IFileService fileService, ILanguageService languageService, IDialogService dialogService, FileExplorerViewModel fileExplorerViewModel)
    {
        _fileService = fileService;
        _languageService = languageService;
        FileExplorerVM = fileExplorerViewModel;
        SaveFileCommand = new RelayCommand(SaveFile);
        OpenFileCommand = new RelayParamsCommand<FileSystemItem>(OpenFile!);
        RunCodeCommand = new RelayCommand(RunCode, CanRunCode);

        InitializeLanguagesAsync().GetAwaiter().GetResult();
    }

    private async Task InitializeLanguagesAsync()
    {
        try
        {
            var languages = await _languageService.GetAllLanguagesAsync();
            foreach (var language in languages)
            {
                Languages.Add(language.Name);
                _languageToRunCommand[language.Name] = language.RunCommand;
            }

            var extensionMap = await _languageService.GetExtensionToLanguageMapAsync();
            foreach (var kvp in extensionMap)
            {
                _extensionToLanguage[kvp.Key] = kvp.Value;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load languages from database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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

        if (SelectedLanguage != null && _languageToRunCommand.TryGetValue(SelectedLanguage, out var runCommand))
        {
            LaunchParameters = SelectedLanguage switch
            {
                "Perl" => $"perl \"{Path.GetFileName(SelectedFilePath)}\"",
                "C#" => GetCSharpLaunchCommand(),
                _ => string.Format(runCommand, SelectedFilePath)
            };
        }
        else
        {
            LaunchParameters = string.Empty;
        }
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
            "JavaScript" => HighlightingManager.Instance.GetDefinition("JavaScript"),
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