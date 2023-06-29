using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NezihBankAPI.Migrations
{
    /// <inheritdoc />
    public partial class Tables_Updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountsAPI_PersonalCustomersAPI_PersonalCustomerId",
                table: "AccountsAPI");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnouncementsAPI_BankManagerAPI_BankManagerId",
                table: "AnnouncementsAPI");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionsAPI_AccountsAPI_AccountId",
                table: "TransactionsAPI");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionsAPI",
                table: "TransactionsAPI");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalCustomersAPI",
                table: "PersonalCustomersAPI");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankManagerAPI",
                table: "BankManagerAPI");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnnouncementsAPI",
                table: "AnnouncementsAPI");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountsAPI",
                table: "AccountsAPI");

            migrationBuilder.RenameTable(
                name: "TransactionsAPI",
                newName: "Transactions");

            migrationBuilder.RenameTable(
                name: "PersonalCustomersAPI",
                newName: "PersonalCustomers");

            migrationBuilder.RenameTable(
                name: "BankManagerAPI",
                newName: "BankManager");

            migrationBuilder.RenameTable(
                name: "AnnouncementsAPI",
                newName: "Announcements");

            migrationBuilder.RenameTable(
                name: "AccountsAPI",
                newName: "Accounts");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionsAPI_AccountId",
                table: "Transactions",
                newName: "IX_Transactions_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_AnnouncementsAPI_BankManagerId",
                table: "Announcements",
                newName: "IX_Announcements_BankManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountsAPI_PersonalCustomerId",
                table: "Accounts",
                newName: "IX_Accounts_PersonalCustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "TransactionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalCustomers",
                table: "PersonalCustomers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankManager",
                table: "BankManager",
                column: "BankManagerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Announcements",
                table: "Announcements",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_PersonalCustomers_PersonalCustomerId",
                table: "Accounts",
                column: "PersonalCustomerId",
                principalTable: "PersonalCustomers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_BankManager_BankManagerId",
                table: "Announcements",
                column: "BankManagerId",
                principalTable: "BankManager",
                principalColumn: "BankManagerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_PersonalCustomers_PersonalCustomerId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_BankManager_BankManagerId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalCustomers",
                table: "PersonalCustomers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankManager",
                table: "BankManager");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Announcements",
                table: "Announcements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "TransactionsAPI");

            migrationBuilder.RenameTable(
                name: "PersonalCustomers",
                newName: "PersonalCustomersAPI");

            migrationBuilder.RenameTable(
                name: "BankManager",
                newName: "BankManagerAPI");

            migrationBuilder.RenameTable(
                name: "Announcements",
                newName: "AnnouncementsAPI");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "AccountsAPI");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_AccountId",
                table: "TransactionsAPI",
                newName: "IX_TransactionsAPI_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Announcements_BankManagerId",
                table: "AnnouncementsAPI",
                newName: "IX_AnnouncementsAPI_BankManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_PersonalCustomerId",
                table: "AccountsAPI",
                newName: "IX_AccountsAPI_PersonalCustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionsAPI",
                table: "TransactionsAPI",
                column: "TransactionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalCustomersAPI",
                table: "PersonalCustomersAPI",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankManagerAPI",
                table: "BankManagerAPI",
                column: "BankManagerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnnouncementsAPI",
                table: "AnnouncementsAPI",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountsAPI",
                table: "AccountsAPI",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountsAPI_PersonalCustomersAPI_PersonalCustomerId",
                table: "AccountsAPI",
                column: "PersonalCustomerId",
                principalTable: "PersonalCustomersAPI",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnnouncementsAPI_BankManagerAPI_BankManagerId",
                table: "AnnouncementsAPI",
                column: "BankManagerId",
                principalTable: "BankManagerAPI",
                principalColumn: "BankManagerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionsAPI_AccountsAPI_AccountId",
                table: "TransactionsAPI",
                column: "AccountId",
                principalTable: "AccountsAPI",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
