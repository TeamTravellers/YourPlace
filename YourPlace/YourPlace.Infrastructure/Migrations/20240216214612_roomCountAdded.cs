using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourPlace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class roomCountAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountInHotel",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountInHotel",
                table: "Rooms");
        }
    }
}
