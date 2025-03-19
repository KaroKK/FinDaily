using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Categ",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Categ_UserId",
                table: "Categ",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CashFlows_CatId",
                table: "CashFlows",
                column: "CatId");

            migrationBuilder.CreateIndex(
                name: "IX_CashFlows_PayId",
                table: "CashFlows",
                column: "PayId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashFlows_Categ_CatId",
                table: "CashFlows",
                column: "CatId",
                principalTable: "Categ",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashFlows_PayWays_PayId",
                table: "CashFlows",
                column: "PayId",
                principalTable: "PayWays",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categ_Users_UserId",
                table: "Categ",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashFlows_Categ_CatId",
                table: "CashFlows");

            migrationBuilder.DropForeignKey(
                name: "FK_CashFlows_PayWays_PayId",
                table: "CashFlows");

            migrationBuilder.DropForeignKey(
                name: "FK_Categ_Users_UserId",
                table: "Categ");

            migrationBuilder.DropIndex(
                name: "IX_Categ_UserId",
                table: "Categ");

            migrationBuilder.DropIndex(
                name: "IX_CashFlows_CatId",
                table: "CashFlows");

            migrationBuilder.DropIndex(
                name: "IX_CashFlows_PayId",
                table: "CashFlows");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Categ");
        }
    }
}
