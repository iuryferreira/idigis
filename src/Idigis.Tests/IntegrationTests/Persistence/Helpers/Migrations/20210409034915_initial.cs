using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Idigis.Tests.IntegrationTests.Persistence.Helpers.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up (MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "churches",
                table => new
                {
                    Id = table.Column<string>("TEXT", nullable: false),
                    Name = table.Column<string>("TEXT", nullable: false),
                    Email = table.Column<string>("TEXT", nullable: false),
                    Password = table.Column<string>("TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_churches", x => x.Id);
                });
            migrationBuilder.CreateTable(
                "members",
                table => new
                {
                    Id = table.Column<string>("TEXT", nullable: false),
                    FullName = table.Column<string>("TEXT", nullable: false),
                    BirthDate = table.Column<DateTime>("TEXT", nullable: true),
                    BaptismDate = table.Column<DateTime>("TEXT", nullable: true),
                    PhoneNumber = table.Column<string>("TEXT", nullable: true),
                    HouseNumber = table.Column<string>("TEXT", nullable: true),
                    Street = table.Column<string>("TEXT", nullable: true),
                    District = table.Column<string>("TEXT", nullable: true),
                    City = table.Column<string>("TEXT", nullable: true),
                    ChurchId = table.Column<string>("TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_members", x => x.Id);
                    table.ForeignKey(
                        "FK_members_churches_ChurchId",
                        x => x.ChurchId,
                        "churches",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                "offers",
                table => new
                {
                    Id = table.Column<string>("TEXT", nullable: false),
                    Value = table.Column<decimal>("TEXT", nullable: false),
                    ChurchId = table.Column<string>("TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_offers", x => x.Id);
                    table.ForeignKey(
                        "FK_offers_churches_ChurchId",
                        x => x.ChurchId,
                        "churches",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                "tithes",
                table => new
                {
                    Id = table.Column<string>("TEXT", nullable: false),
                    Value = table.Column<decimal>("TEXT", nullable: false),
                    Date = table.Column<DateTime>("TEXT", nullable: false),
                    MemberId = table.Column<string>("TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tithes", x => x.Id);
                    table.ForeignKey(
                        "FK_tithes_members_MemberId",
                        x => x.MemberId,
                        "members",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateIndex(
                "IX_members_ChurchId",
                "members",
                "ChurchId");
            migrationBuilder.CreateIndex(
                "IX_offers_ChurchId",
                "offers",
                "ChurchId");
            migrationBuilder.CreateIndex(
                "IX_tithes_MemberId",
                "tithes",
                "MemberId");
        }

        protected override void Down (MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "offers");
            migrationBuilder.DropTable(
                "tithes");
            migrationBuilder.DropTable(
                "members");
            migrationBuilder.DropTable(
                "churches");
        }
    }
}
