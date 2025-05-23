using CodeEditor.Core.Abstractions.Data;
using CodeEditor.Core.Entities;
using CodeEditor.Infrastructure.Data;

namespace CodeEditor.Infrastructure.Repositories;

public class AppUnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly Lazy<IRepository<ErrorLog>> _errorLogRepository = 
        new(() => new AppRepository<ErrorLog>(context));
    private readonly Lazy<IRepository<Language>> _languagesRepository = 
        new(() => new AppRepository<Language>(context));
    
    public IRepository<ErrorLog> ErrorLogsRepository => _errorLogRepository.Value;
    public IRepository<Language> LanguagesRepository => _languagesRepository.Value;

    public async Task SaveAllAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}