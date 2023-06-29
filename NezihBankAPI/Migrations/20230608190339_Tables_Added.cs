using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NezihBankAPI.Migrations
{
    /// <inheritdoc />
    public partial class Tables_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankManagerAPI",
                columns: table => new
                {
                    BankManagerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankManagerAPI", x => x.BankManagerId);
                });

            migrationBuilder.CreateTable(
                name: "PersonalCustomersAPI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalCustomersAPI", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementsAPI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankManagerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementsAPI", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnouncementsAPI_BankManagerAPI_BankManagerId",
                        column: x => x.BankManagerId,
                        principalTable: "BankManagerAPI",
                        principalColumn: "BankManagerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountsAPI",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IbanNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PersonalCustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsAPI", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_AccountsAPI_PersonalCustomersAPI_PersonalCustomerId",
                        column: x => x.PersonalCustomerId,
                        principalTable: "PersonalCustomersAPI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionsAPI",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionCategory = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionsAPI", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_TransactionsAPI_AccountsAPI_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AccountsAPI",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountsAPI_PersonalCustomerId",
                table: "AccountsAPI",
                column: "PersonalCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementsAPI_BankManagerId",
                table: "AnnouncementsAPI",
                column: "BankManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionsAPI_AccountId",
                table: "TransactionsAPI",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnouncementsAPI");

            migrationBuilder.DropTable(
                name: "TransactionsAPI");

            migrationBuilder.DropTable(
                name: "BankManagerAPI");

            migrationBuilder.DropTable(
                name: "AccountsAPI");

            migrationBuilder.DropTable(
                name: "PersonalCustomersAPI");
        }
    }
}
