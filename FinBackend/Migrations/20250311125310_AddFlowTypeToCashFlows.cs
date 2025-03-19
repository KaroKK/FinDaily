using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinBackend.Migrations
{
    public partial class AddFlowTypeToCashFlows : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashFlows_Categ_CatId",
                table: "CashFlows");

            migrationBuilder.DropForeignKey(
                name: "FK_CashFlows_PayWays_PayId",
                table: "CashFlows");

            migrationBuilder.DropForeignKey(
                name: "FK_CashFlows_Users_UserId",
                table: "CashFlows");

            migrationBuilder.DropIndex(
                name: "IX_CashFlows_CatId",
                table: "CashFlows");

            migrationBuilder.DropIndex(
                name: "IX_CashFlows_PayId",
                table: "CashFlows");

            migrationBuilder.DropIndex(
                name: "IX_CashFlows_UserId",
                table: "CashFlows");

            migrationBuilder.AddColumn<string>(
                name: "FlowType",
                table: "CashFlows",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlowType",
                table: "CashFlows");

            migrationBuilder.CreateIndex(
                name: "IX_CashFlows_CatId",
                table: "CashFlows",
                column: "CatId");

            migrationBuilder.CreateIndex(
                name: "IX_CashFlows_PayId",
                table: "CashFlows",
                column: "PayId");

            migrationBuilder.CreateIndex(
                name: "IX_CashFlows_UserId",
                table: "CashFlows",
                column: "UserId");

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
                name: "FK_CashFlows_Users_UserId",
                table: "CashFlows",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
