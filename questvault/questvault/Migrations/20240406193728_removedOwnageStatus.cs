using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace questvault.Migrations
{
    /// <inheritdoc />
    public partial class removedOwnageStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ownage",
                table: "GameLog");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ownage",
                table: "GameLog",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
