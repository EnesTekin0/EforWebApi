using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EforWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumnToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EforId",
                table: "Efforts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EforId",
                table: "Efforts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
