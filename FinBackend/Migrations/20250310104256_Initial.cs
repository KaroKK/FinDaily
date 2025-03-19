using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinBackend.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categ",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CatName = table.Column<string>(type: "text", nullable: false),
                    CatInfo = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categ", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PassHash = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActionLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    LogAction = table.Column<string>(type: "text", nullable: false),
                    LogDetails = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PayWays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    PayLabel = table.Column<string>(type: "text", nullable: false),
                    PayInfo = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayWays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayWays_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanBuds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CatId = table.Column<int>(type: "integer", nullable: false),
                    LimitAmt = table.Column<decimal>(type: "numeric", nullable: false),
                    PeriodTxt = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanBuds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanBuds_Categ_CatId",
                        column: x => x.CatId,
                        principalTable: "Categ",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanBuds_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CashFlows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FlowDesc = table.Column<string>(type: "text", nullable: false),
                    FlowAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    FlowDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CatId = table.Column<int>(type: "integer", nullable: true),
                    PayId = table.Column<int>(type: "integer", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashFlows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashFlows_Categ_CatId",
                        column: x => x.CatId,
                        principalTable: "Categ",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CashFlows_PayWays_PayId",
                        column: x => x.PayId,
                        principalTable: "PayWays",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CashFlows_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionLogs_UserId",
                table: "ActionLogs",
                column: "UserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_PayWays_UserId",
                table: "PayWays",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanBuds_CatId",
                table: "PlanBuds",
                column: "CatId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanBuds_UserId",
                table: "PlanBuds",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionLogs");

            migrationBuilder.DropTable(
                name: "CashFlows");

            migrationBuilder.DropTable(
                name: "PlanBuds");

            migrationBuilder.DropTable(
                name: "PayWays");

            migrationBuilder.DropTable(
                name: "Categ");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
