using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CodeEditor.Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var dbPath = Path.Combine(baseDir, "..", "..", "..", "..", "code-editor.db");
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>();
        options.UseSqlite($"Data Source={dbPath}");

        return new ApplicationDbContext(options.Options);
    }
}