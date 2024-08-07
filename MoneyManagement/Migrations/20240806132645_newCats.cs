using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManagement.Migrations
{
    /// <inheritdoc />
    public partial class newCats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Transactions",
                newName: "CategoryName");

            migrationBuilder.AddColumn<bool>(
                name: "Expense",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Art Supplies",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Books",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Car & Fuel",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Cashback",
                column: "Expense",
                value: false);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Clothing",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Games",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Groceries",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Health",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "House",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Household",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Hygiene",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Insurance",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Other Fees",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Other Hobbies",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Phone",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Present",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Salary",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Stream and Tv",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Taxes",
                column: "Expense",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Transer",
                column: "Expense",
                value: false);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Name", "Expense" },
                values: new object[] { "Income", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Name",
                keyValue: "Income");

            migrationBuilder.DropColumn(
                name: "Expense",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Transactions",
                newName: "Category");
        }
    }
}
