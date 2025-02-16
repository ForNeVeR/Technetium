using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Technetium.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExternalId = table.Column<string>(type: "TEXT", nullable: true),
                    ScheduledAtInstant = table.Column<string>(type: "TEXT", nullable: true),
                    ScheduledAtTimeZoneId = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Order = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskRecords", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskRecords_ExternalId",
                table: "TaskRecords",
                column: "ExternalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskRecords");
        }
    }
}
