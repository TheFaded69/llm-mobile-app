using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Main.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SessionStatus",
                table: "Sessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionStatus",
                table: "Sessions");
        }
    }
}
