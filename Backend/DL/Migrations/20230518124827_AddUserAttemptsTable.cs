using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DL.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAttemptsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAttempts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Mark = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    SubscriptionId = table.Column<string>(type: "longtext", nullable: false),
                    TestId = table.Column<string>(type: "longtext", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAttempts", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAttempts");
        }
    }
}
