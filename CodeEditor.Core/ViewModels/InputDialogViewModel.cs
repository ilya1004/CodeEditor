using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CodeEditor.Core.Commands;

namespace CodeEditor.Core.ViewModels;

public class InputDialogViewModel : INotifyPropertyChanged
{
    private string _prompt = string.Empty;
    public string Prompt
    {
        get => _prompt;
        set
        {
            _prompt = value;
            OnPropertyChanged();
        }
    }

    private string _inputText = string.Empty;
    public string InputText
    {
        get => _inputText;
        set
        {
            _inputText = value;
            OnPropertyChanged();
        }
    }

    public ICommand OkCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    public InputDialogViewModel()
    {
        OkCommand = new RelayCommand(() => { });
        CancelCommand = new RelayCommand(() => { });
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}