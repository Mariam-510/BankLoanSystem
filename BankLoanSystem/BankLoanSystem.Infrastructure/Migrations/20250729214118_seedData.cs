using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLoanSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "CodeGeneratedAt", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmationCode", "EmailConfirmed", "FirstName", "IsDeleted", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PasswordResetCode", "PhoneNumber", "PhoneNumberConfirmed", "ResetCodeGeneratedAt", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "779af355-c7c9-4935-ad2f-85c324825ff2", 0, null, "223d0eed-1b02-44c7-9bae-baae3508f127", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin1@gmail.com", null, true, "", false, null, true, null, "ADMIN1@GMAIL.COM", "ADMIN1@GMAIL.COM", "AQAAAAIAAYagAAAAEG7/LUrcXKZaK3krqZoKCu3SYiqlupsTeHA4lnjWi0QONNvF0hF5zBNXYhZKBOwh/Q==", null, null, false, null, "31012730-e125-4ecb-8811-38744d3a6160", false, "admin1@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "266c6ce3-1f56-4601-9d45-5d9b87b01b30", "779af355-c7c9-4935-ad2f-85c324825ff2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "266c6ce3-1f56-4601-9d45-5d9b87b01b30", "779af355-c7c9-4935-ad2f-85c324825ff2" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "779af355-c7c9-4935-ad2f-85c324825ff2");
        }
    }
}
