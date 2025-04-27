using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeEditor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedGoLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "DocumentationUrl", "Extensions", "IsExecutable", "Name", "RunCommand" },
                values: new object[] { 11, "https://go.dev/doc/", ".go", true, "Go", "go run \"{0}\"" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 11);
        }
    }
}
