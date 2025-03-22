using System.Windows;
using CodeEditor.Core.ViewModels;

namespace CodeEditor.Views.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}