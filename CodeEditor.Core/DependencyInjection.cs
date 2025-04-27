using CodeEditor.Core.Abstractions.Services;
using CodeEditor.Core.Services;
using CodeEditor.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CodeEditor.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<FileExplorerViewModel>();
        services.AddSingleton<InputDialogViewModel>();
        
        services.AddTransient<ILanguageService, LanguageService>();

        return services;
    }
}