using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace questvault.Migrations
{
    /// <inheritdoc />
    public partial class adminStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePhotoPath",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "AccessInstances",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessInstances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogginInstances",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogginInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogginInstances_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogginInstances_UserId",
                table: "LogginInstances",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessInstances");

            migrationBuilder.DropTable(
                name: "LogginInstances");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePhotoPath",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
