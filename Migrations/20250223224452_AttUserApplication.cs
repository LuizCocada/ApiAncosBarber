using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AncosBarber.Migrations
{
    /// <inheritdoc />
    public partial class AttUserApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BarberShopId",
                table: "AspNetUsers",
                column: "BarberShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_BarberShops_BarberShopId",
                table: "AspNetUsers",
                column: "BarberShopId",
                principalTable: "BarberShops",
                principalColumn: "BarberShopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BarberShops_BarberShopId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BarberShopId",
                table: "AspNetUsers");
        }
    }
}
