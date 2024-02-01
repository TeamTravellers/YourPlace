using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourPlace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class categoriesTypesCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Suggestions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Pricing",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_Suggestions_UserID",
                table: "Suggestions",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomsAvailability_HotelID",
                table: "RoomsAvailability",
                column: "HotelID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_HotelID",
                table: "Reservations",
                column: "HotelID");

            migrationBuilder.CreateIndex(
                name: "IX_Images_HotelID",
                table: "Images",
                column: "HotelID");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_HotelID",
                table: "Categories",
                column: "HotelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Hotels_HotelID",
                table: "Categories",
                column: "HotelID",
                principalTable: "Hotels",
                principalColumn: "HotelID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Hotels_HotelID",
                table: "Images",
                column: "HotelID",
                principalTable: "Hotels",
                principalColumn: "HotelID",
                onDelete: ReferentialAction.Restrict);

            

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Hotels_HotelID",
                table: "Reservations",
                column: "HotelID",
                principalTable: "Hotels",
                principalColumn: "HotelID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomsAvailability_Rooms_HotelID",
                table: "RoomsAvailability",
                column: "HotelID",
                principalTable: "Rooms",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_AspNetUsers_UserID",
                table: "Suggestions",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Hotels_HotelID",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Hotels_HotelID",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Hotels_HotelID1",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Hotels_HotelID",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomsAvailability_Rooms_HotelID",
                table: "RoomsAvailability");

            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_AspNetUsers_UserID",
                table: "Suggestions");

            migrationBuilder.DropIndex(
                name: "IX_Suggestions_UserID",
                table: "Suggestions");

            migrationBuilder.DropIndex(
                name: "IX_RoomsAvailability_HotelID",
                table: "RoomsAvailability");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_HotelID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Images_HotelID",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_HotelID1",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Categories_HotelID",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "HotelID1",
                table: "Images");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Suggestions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Pricing",
                table: "Categories",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
