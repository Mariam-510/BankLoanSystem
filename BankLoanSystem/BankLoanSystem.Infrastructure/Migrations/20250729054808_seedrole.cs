using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankLoanSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "266c6ce3-1f56-4601-9d45-5d9b87b01b30", "266c6ce3-1f56-4601-9d45-5d9b87b01b30", "Admin", "ADMIN" },
                    { "cdb95803-0507-4a46-9c14-0c3b3055c9be", "cdb95803-0507-4a46-9c14-0c3b3055c9be", "Client", "CLIENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "266c6ce3-1f56-4601-9d45-5d9b87b01b30");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cdb95803-0507-4a46-9c14-0c3b3055c9be");
        }
    }
}
