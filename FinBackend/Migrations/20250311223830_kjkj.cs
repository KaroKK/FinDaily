using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinBackend.Migrations
{
    /// <inheritdoc />
    public partial class kjkj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashFlows_Categ_CategoryId",
                table: "CashFlows");

            migrationBuilder.DropForeignKey(
                name: "FK_CashFlows_PayWays_PayWayId",
                table: "CashFlows");

            migrationBuilder.DropForeignKey(
                name: "FK_CashFlows_Users_UserId",
                table: "CashFlows");

            migrationBuilder.DropIndex(
                name: "IX_CashFlows_CategoryId",
                table: "CashFlows");

            migrationBuilder.DropIndex(
                name: "IX_CashFlows_PayWayId",
                table: "CashFlows");

            migrationBuilder.DropIndex(
                name: "IX_CashFlows_UserId",
                table: "CashFlows");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PayWays");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "CashFlows");

            migrationBuilder.DropColumn(
                name: "PayWayId",
                table: "CashFlows");

            migrationBuilder.AlterColumn<string>(
                name: "PayInfo",
                table: "PayWays",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

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

            migrationBuilder.DropIndex(
                name: "IX_CashFlows_CatId",
                table: "CashFlows");

            migrationBuilder.DropIndex(
                name: "IX_CashFlows_PayId",
                table: "CashFlows");

            migrationBuilder.AlterColumn<string>(
                name: "PayInfo",
                table: "PayWays",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "PayWays",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "CashFlows",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PayWayId",
                table: "CashFlows",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CashFlows_CategoryId",
                table: "CashFlows",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CashFlows_PayWayId",
                table: "CashFlows",
                column: "PayWayId");

            migrationBuilder.CreateIndex(
                name: "IX_CashFlows_UserId",
                table: "CashFlows",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashFlows_Categ_CategoryId",
                table: "CashFlows",
                column: "CategoryId",
                principalTable: "Categ",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CashFlows_PayWays_PayWayId",
                table: "CashFlows",
                column: "PayWayId",
                principalTable: "PayWays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
