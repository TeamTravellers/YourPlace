using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourPlace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class check : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomsAvailability_Rooms_HotelID",
                table: "RoomsAvailability");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomsAvailability_Hotels_HotelID",
                table: "RoomsAvailability",
                column: "HotelID",
                principalTable: "Hotels",
                principalColumn: "HotelID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomsAvailability_Hotels_HotelID",
                table: "RoomsAvailability");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomsAvailability_Rooms_HotelID",
                table: "RoomsAvailability",
                column: "HotelID",
                principalTable: "Rooms",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
