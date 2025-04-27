using System.IO;
using CodeEditor.Core.Abstractions.Data;
using CodeEditor.Core.Abstractions.Services;
using CodeEditor.Infrastructure.Data;
using CodeEditor.Infrastructure.Repositories;
using CodeEditor.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeEditor.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var dbPath = Path.Combine(baseDir, "..", "..", "..", "..", "code-editor.db");
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        services.AddTransient<IUnitOfWork, AppUnitOfWork>();
        
        services.AddTransient<IFileService, FileService>();
        
        return services;
    }
}