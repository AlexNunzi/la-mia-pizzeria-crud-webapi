using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace la_mia_pizzeria_static.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryPizzaRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "Pizza",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pizza_CategoryId",
                table: "Pizza",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pizza_Categories_CategoryId",
                table: "Pizza",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pizza_Categories_CategoryId",
                table: "Pizza");

            migrationBuilder.DropIndex(
                name: "IX_Pizza_CategoryId",
                table: "Pizza");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Pizza");
        }
    }
}
