using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Idigis.Core.Persistence.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "churches",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_churches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "members",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    BaptismDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    HouseNumber = table.Column<string>(type: "text", nullable: true),
                    Street = table.Column<string>(type: "text", nullable: true),
                    District = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    ChurchId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_members_churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "churches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "offers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    ChurchId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_offers_churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "churches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tithes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    MemberId = table.Column<string>(type: "text", nullable: false),
                    ChurchModelId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tithes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tithes_churches_ChurchModelId",
                        column: x => x.ChurchModelId,
                        principalTable: "churches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tithes_members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_members_ChurchId",
                table: "members",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_offers_ChurchId",
                table: "offers",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_tithes_ChurchModelId",
                table: "tithes",
                column: "ChurchModelId");

            migrationBuilder.CreateIndex(
                name: "IX_tithes_MemberId",
                table: "tithes",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "offers");

            migrationBuilder.DropTable(
                name: "tithes");

            migrationBuilder.DropTable(
                name: "members");

            migrationBuilder.DropTable(
                name: "churches");
        }
    }
}
