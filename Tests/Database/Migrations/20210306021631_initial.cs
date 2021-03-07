using Microsoft.EntityFrameworkCore.Migrations;

namespace Tests.Database.Migrations
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
        }

        protected override void Down (MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "churches");
        }
    }
}
