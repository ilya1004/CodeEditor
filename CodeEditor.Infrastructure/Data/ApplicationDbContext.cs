using CodeEditor.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeEditor.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ErrorLog> ErrorLogs { get; set; }
    public DbSet<Language> Languages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Начальные данные для таблицы Languages
        modelBuilder.Entity<Language>().HasData(
            new Language
            {
                Id = 1,
                Name = "Python",
                Extensions = ".py",
                RunCommand = "python \"{0}\""
            },
            new Language
            {
                Id = 2,
                Name = "Perl",
                Extensions = ".pl",
                RunCommand = "perl \"{0}\""
            },
            new Language
            {
                Id = 3,
                Name = "C#",
                Extensions = ".cs,.csproj",
                RunCommand = "dotnet run --project \"{0}\""
            },
            new Language
            {
                Id = 4,
                Name = "JavaScript",
                Extensions = ".js",
                RunCommand = "node \"{0}\""
            }
        );
    }
}