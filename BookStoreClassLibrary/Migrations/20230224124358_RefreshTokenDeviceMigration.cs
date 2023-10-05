using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenDeviceMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5525d395-236d-4acf-996f-a01d736dd6bc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73f3edd9-3ca7-42d4-ae97-841c1caf1930");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f307b45-361b-489a-8ce1-9693b2b9f0bf");

            migrationBuilder.AddColumn<string>(
                name: "Device",
                table: "RefreshTokens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3f5761d0-7f3e-4e75-b253-ab0465a28fe0", "bd9b2b43-bc4a-4d2c-bda1-eeda85894ccb", "ADMINISTRATOR", "ADMINISTRATOR" },
                    { "8d9d016c-b520-4288-ba18-654f206488f6", "be457422-ace5-4ed4-abde-17861c4106ef", "CUSTOMER", "CUSTOMER" },
                    { "d513ac5c-5717-419c-b8b8-b289297da2a0", "2810a4f0-bac0-4f4c-90e3-7aa5855bf398", "LIBRARIAN", "LIBRARIAN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f5761d0-7f3e-4e75-b253-ab0465a28fe0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d9d016c-b520-4288-ba18-654f206488f6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d513ac5c-5717-419c-b8b8-b289297da2a0");

            migrationBuilder.DropColumn(
                name: "Device",
                table: "RefreshTokens");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5525d395-236d-4acf-996f-a01d736dd6bc", "c7df8b1b-8b24-41cd-82d5-4616a9d99822", "CUSTOMER", "CUSTOMER" },
                    { "73f3edd9-3ca7-42d4-ae97-841c1caf1930", "ee8d6603-e41b-4dd0-9fe9-bedfc19f61e9", "LIBRARIAN", "LIBRARIAN" },
                    { "8f307b45-361b-489a-8ce1-9693b2b9f0bf", "c326da1b-7e99-48fb-8088-d9128cd305a5", "ADMINISTRATOR", "ADMINISTRATOR" }
                });
        }
    }
}
