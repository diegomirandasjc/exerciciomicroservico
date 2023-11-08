using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountsBalanceDates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FK_Account = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountDescription = table.Column<string>(type: "text", nullable: false),
                    AccountBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsBalanceDates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountsOperationDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FK_Account = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountDescription = table.Column<string>(type: "text", nullable: false),
                    AccountBalanceBeforeOperation = table.Column<decimal>(type: "numeric", nullable: false),
                    AccountBalanceAfeterOperation = table.Column<decimal>(type: "numeric", nullable: false),
                    OperationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OperationAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    OperationUserId = table.Column<string>(type: "text", nullable: false),
                    OperationUserName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsOperationDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountsBalanceDates");

            migrationBuilder.DropTable(
                name: "AccountsOperationDetails");
        }
    }
}
