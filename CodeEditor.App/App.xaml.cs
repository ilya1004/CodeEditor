using System.Windows;
using System.Windows.Threading;
using CodeEditor.Core.Abstractions;
using CodeEditor.Core.Abstractions.Data;
using CodeEditor.Core.Entities;
using CodeEditor.Core.ViewModels;
using CodeEditor.Infrastructure;
using CodeEditor.Infrastructure.Services;
using CodeEditor.Views.Views;
using Microsoft.Extensions.DependencyInjection;

namespace CodeEditor.App;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        DispatcherUnhandledException += OnDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddInfrastructure();
        
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<FileExplorerViewModel>();
        services.AddSingleton<MainWindow>();

        services.AddTransient<IFileService, FileService>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
    
    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show($"Произошла ошибка: {e.Exception.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        LogError(e.Exception);
        e.Handled = true;
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            LogError(ex);
        }
    }

    private void LogError(Exception ex)
    {
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();

        var errorLog = new ErrorLog
        {
            Timestamp = DateTime.Now,
            Message = ex.Message,
            StackTrace = ex.StackTrace
        };

        unitOfWork.ErrorLogRepository.AddAsync(errorLog);
        unitOfWork.SaveAllAsync();
    }
}