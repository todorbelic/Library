using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d4465bc-8b47-4cf9-b4f4-5ecf06c8018e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e0aa492-2d6d-48e4-9d42-178bd6fdaec7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f9872d27-0255-4800-88b6-509ecd9abe62");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4d4465bc-8b47-4cf9-b4f4-5ecf06c8018e", "8d78c6ec-7d52-40dd-bf04-057db1e6e506", "ADMINISTRATOR", "ADMINISTRATOR" },
                    { "5e0aa492-2d6d-48e4-9d42-178bd6fdaec7", "aa6f79e8-8b8f-4077-9755-fc3187474b4c", "LIBRARIAN", "LIBRARIAN" },
                    { "f9872d27-0255-4800-88b6-509ecd9abe62", "a5298140-2534-4549-957f-fba3d7c315ef", "CUSTOMER", "CUSTOMER" }
                });
        }
    }
}
