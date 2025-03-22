using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CodeEditor.Core.Abstractions;
using CodeEditor.Core.Commands;
using Microsoft.Win32;

namespace CodeEditor.Core.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly IFileService _fileService;

    public MainWindowViewModel(IFileService fileService)
    {
        _fileService = fileService;
        OpenFileCommand = new RelayCommand(OpenFile);
        SaveFileCommand = new RelayCommand(SaveFile);
    }

    // Команда для открытия файла
    public ICommand OpenFileCommand { get; }
    private void OpenFile()
    {
        var openFileDialog = new OpenFileDialog();
        if (openFileDialog.ShowDialog() == true)
        {
            CodeText = _fileService.ReadFile(openFileDialog.FileName);
        }
    }

    // Команда для сохранения файла
    public ICommand SaveFileCommand { get; }
    private void SaveFile()
    {
        var saveFileDialog = new SaveFileDialog();
        if (saveFileDialog.ShowDialog() == true)
        {
            _fileService.SaveFile(saveFileDialog.FileName, CodeText);
        }
    }

    // Свойство для текста
    private string _codeText = string.Empty;
    
    public string CodeText
    {
        get => _codeText;
        set
        {
            _codeText = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}