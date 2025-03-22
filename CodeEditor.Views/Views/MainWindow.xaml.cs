using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CodeEditor.Core.Models;
using CodeEditor.Core.ViewModels;

namespace CodeEditor.Views.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void ListViewItem_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is ListViewItem { DataContext: FileSystemItem fileSystemItem })
        {
            if (fileSystemItem.IsDirectory)
            {
                var viewModel = DataContext as MainWindowViewModel;
                viewModel?.FileExplorerVM.OpenItemCommand.Execute(fileSystemItem);
            }
            else
            {
                var viewModel = DataContext as MainWindowViewModel;
                viewModel?.OpenFileCommand.Execute(fileSystemItem);
            }
        }
    }
}