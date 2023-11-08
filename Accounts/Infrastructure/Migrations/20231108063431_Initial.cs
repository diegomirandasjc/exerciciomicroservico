using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountOperationPerformedMessageOutbox",
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
                    table.PrimaryKey("PK_AccountOperationPerformedMessageOutbox", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountOperationPerformedMessageOutbox_Accounts_FK_Account",
                        column: x => x.FK_Account,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountsMovimentations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OperationType = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    BalanceSnapShot = table.Column<decimal>(type: "numeric", nullable: false),
                    FK_Account = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsMovimentations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountsMovimentations_Accounts_FK_Account",
                        column: x => x.FK_Account,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountOperationPerformedMessageOutbox_FK_Account",
                table: "AccountOperationPerformedMessageOutbox",
                column: "FK_Account");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsMovimentations_FK_Account",
                table: "AccountsMovimentations",
                column: "FK_Account");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountOperationPerformedMessageOutbox");

            migrationBuilder.DropTable(
                name: "AccountsMovimentations");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
