using CodeEditor.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeEditor.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ErrorLog> ErrorLogs { get; set; }
}