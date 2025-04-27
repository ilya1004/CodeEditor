using System.Windows;
using CodeEditor.Core.Commands;
using CodeEditor.Core.ViewModels;

namespace CodeEditor.Views.Views;

public partial class InputDialogWindow : Window
{
    private readonly InputDialogViewModel _viewModel;

    public string Result => _viewModel.InputText;

    public InputDialogWindow(string prompt)
    {
        InitializeComponent();
        _viewModel = new InputDialogViewModel { Prompt = prompt };
        DataContext = _viewModel;

        _viewModel.OkCommand = new RelayCommand(() =>
        {
            DialogResult = true;
            Close();
        });

        _viewModel.CancelCommand = new RelayCommand(() =>
        {
            DialogResult = false;
            Close();
        });
    }
}