using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CodeEditor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErrorLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    StackTrace = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Extensions = table.Column<string>(type: "TEXT", nullable: false),
                    RunCommand = table.Column<string>(type: "TEXT", nullable: false),
                    IsExecutable = table.Column<bool>(type: "INTEGER", nullable: false),
                    DocumentationUrl = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "DocumentationUrl", "Extensions", "IsExecutable", "Name", "RunCommand" },
                values: new object[,]
                {
                    { 1, "https://docs.python.org/3/", ".py", true, "Python", "python \"{0}\"" },
                    { 2, "https://perldoc.perl.org/", ".pl", true, "Perl", "perl \"{0}\"" },
                    { 3, "https://learn.microsoft.com/en-us/dotnet/csharp/", ".cs,.csproj", true, "C#", "dotnet run --project \"{0}\"" },
                    { 4, "https://developer.mozilla.org/en-US/docs/Web/JavaScript", ".js", true, "JavaScript", "node \"{0}\"" },
                    { 5, "https://developer.mozilla.org/en-US/docs/Web/HTML", ".html,.htm", false, "HTML", "" },
                    { 6, "https://developer.mozilla.org/en-US/docs/Web/CSS", ".css", false, "CSS", "" },
                    { 7, "https://www.w3schools.com/sql/", ".sql", false, "SQL", "" },
                    { 8, "https://www.json.org/json-en.html", ".json", false, "JSON", "" },
                    { 9, "https://yaml.org/", ".yml,.yaml", false, "YAML", "" },
                    { 10, "https://www.markdownguide.org/", ".md,.markdown", false, "Markdown", "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorLogs");

            migrationBuilder.DropTable(
                name: "Languages");
        }
    }
}
