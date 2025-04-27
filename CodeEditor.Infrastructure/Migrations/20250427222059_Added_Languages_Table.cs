using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CodeEditor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_Languages_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Extensions = table.Column<string>(type: "TEXT", nullable: false),
                    RunCommand = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Extensions", "Name", "RunCommand" },
                values: new object[,]
                {
                    { 1, ".py", "Python", "python \"{0}\"" },
                    { 2, ".pl", "Perl", "perl \"{0}\"" },
                    { 3, ".cs,.csproj", "C#", "dotnet run --project \"{0}\"" },
                    { 4, ".js", "JavaScript", "node \"{0}\"" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Languages");
        }
    }
}
