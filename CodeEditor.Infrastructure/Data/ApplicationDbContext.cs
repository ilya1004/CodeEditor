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
                RunCommand = "python \"{0}\"",
                IsExecutable = true,
                DocumentationUrl = "https://docs.python.org/3/"
            },
            new Language
            {
                Id = 2,
                Name = "Perl",
                Extensions = ".pl",
                RunCommand = "perl \"{0}\"",
                IsExecutable = true,
                DocumentationUrl = "https://perldoc.perl.org/"
            },
            new Language
            {
                Id = 3,
                Name = "C#",
                Extensions = ".cs,.csproj",
                RunCommand = "dotnet run --project \"{0}\"",
                IsExecutable = true,
                DocumentationUrl = "https://learn.microsoft.com/en-us/dotnet/csharp/"
            },
            new Language
            {
                Id = 4,
                Name = "JavaScript",
                Extensions = ".js",
                RunCommand = "node \"{0}\"",
                IsExecutable = true,
                DocumentationUrl = "https://developer.mozilla.org/en-US/docs/Web/JavaScript"
            },
            new Language
            {
                Id = 5,
                Name = "HTML",
                Extensions = ".html,.htm",
                RunCommand = "",
                IsExecutable = false,
                DocumentationUrl = "https://developer.mozilla.org/en-US/docs/Web/HTML"
            },
            new Language
            {
                Id = 6,
                Name = "CSS",
                Extensions = ".css",
                RunCommand = "",
                IsExecutable = false,
                DocumentationUrl = "https://developer.mozilla.org/en-US/docs/Web/CSS"
            },
            new Language
            {
                Id = 7,
                Name = "SQL",
                Extensions = ".sql",
                RunCommand = "",
                IsExecutable = false,
                DocumentationUrl = "https://www.w3schools.com/sql/"
            },
            new Language
            {
                Id = 8,
                Name = "JSON",
                Extensions = ".json",
                RunCommand = "",
                IsExecutable = false,
                DocumentationUrl = "https://www.json.org/json-en.html"
            },
            new Language
            {
                Id = 9,
                Name = "YAML",
                Extensions = ".yml,.yaml",
                RunCommand = "",
                IsExecutable = false,
                DocumentationUrl = "https://yaml.org/"
            },
            new Language
            {
                Id = 10,
                Name = "Markdown",
                Extensions = ".md,.markdown",
                RunCommand = "",
                IsExecutable = false,
                DocumentationUrl = "https://www.markdownguide.org/"
            },
            new Language
            {
                Id = 11,
                Name = "Go",
                Extensions = ".go",
                RunCommand = "go run \"{0}\"",
                IsExecutable = true,
                DocumentationUrl = "https://go.dev/doc/"
            }
        );
    }
}