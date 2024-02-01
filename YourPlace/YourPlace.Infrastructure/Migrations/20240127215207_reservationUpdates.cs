using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourPlace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class reservationUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HotelID",
                table: "Suggestions");

            migrationBuilder.RenameColumn(
                name: "RoomID",
                table: "Reservations",
                newName: "HotelID");

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Suggestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ReservationID",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ReservationID",
                table: "Rooms",
                column: "ReservationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Reservations_ReservationID",
                table: "Rooms",
                column: "ReservationID",
                principalTable: "Reservations",
                principalColumn: "ReservationID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Reservations_ReservationID",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_ReservationID",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Suggestions");

            migrationBuilder.DropColumn(
                name: "ReservationID",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "HotelID",
                table: "Reservations",
                newName: "RoomID");

            migrationBuilder.AddColumn<int>(
                name: "HotelID",
                table: "Suggestions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
