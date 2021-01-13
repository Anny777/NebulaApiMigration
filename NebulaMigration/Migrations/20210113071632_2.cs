using Microsoft.EntityFrameworkCore.Migrations;

namespace NebulaMigration.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c3126bd5-811c-4113-984f-eea064363382", "06de05a0-4014-4fb8-ae6e-68bedd3ea953", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Access_token", "ConcurrencyStamp", "Email", "EmailConfirmed", "Expires", "Expires_in", "Issued", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OperatorId", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Token_type", "TwoFactorEnabled", "UserName" },
                values: new object[] { "55b3ea7d-14bd-46f0-947d-534e22a0af13", 0, null, "eceee2fb-7790-46bd-adb5-ad86fb6e0979", "admin@nebula.com", true, null, 0, null, false, null, "ADMIN@NEBULA.COM", "admin@nebula.com", 1, "AQAAAAEAACcQAAAAEF3jNG2nKXcx6MqnB0yVOL1iGBFWrZE9N1TKq6jhMmDzjHH8QB8/84qPtnmOfrlyTw==", null, false, "", null, false, "admin@nebula.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "55b3ea7d-14bd-46f0-947d-534e22a0af13", "c3126bd5-811c-4113-984f-eea064363382" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "55b3ea7d-14bd-46f0-947d-534e22a0af13", "c3126bd5-811c-4113-984f-eea064363382" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3126bd5-811c-4113-984f-eea064363382");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "55b3ea7d-14bd-46f0-947d-534e22a0af13");
        }
    }
}
