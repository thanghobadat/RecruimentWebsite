using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class addColumnForChatTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("5a5a6b22-c16c-488f-8c2c-36741b4ff55e"));

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("b2555e17-3b7d-48a8-8f5c-4becc36d0eed"));

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("c7b92a34-a345-4273-a0e8-b16032c08ace"));

            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("c7b92a34-a345-4273-a0e8-b16032c08ace"), new Guid("8841e141-d060-4fd7-b55b-903e64dd757a") });

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("8841e141-d060-4fd7-b55b-903e64dd757a"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 3, 27, 16, 47, 43, 81, DateTimeKind.Local).AddTicks(2721),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 3, 7, 16, 11, 0, 505, DateTimeKind.Local).AddTicks(7195));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "CurriculumVitaes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 3, 27, 16, 47, 43, 77, DateTimeKind.Local).AddTicks(1832),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 3, 7, 16, 11, 0, 500, DateTimeKind.Local).AddTicks(7310));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Chats",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 3, 27, 16, 47, 43, 54, DateTimeKind.Local).AddTicks(9173),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 3, 7, 16, 11, 0, 475, DateTimeKind.Local).AddTicks(1218));

            migrationBuilder.AddColumn<string>(
                name: "Performer",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("275dbac8-a1d2-484d-83f2-07718316271c"), "75e221b4-cbf8-4dd0-951c-10396e39104f", "Company role", "company", "company" },
                    { new Guid("72ae53ec-a8cd-4a42-a6c5-827212e258f7"), "07535ff3-ab6a-47de-b522-321ff53d96be", "User role", "user", "user" },
                    { new Guid("25aad933-26f6-4343-a506-9fb41e90be1d"), "9d6d9fea-3f4e-4eac-951d-a7388f0825b6", "Admin role", "admin", "admin" }
                });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("25aad933-26f6-4343-a506-9fb41e90be1d"), new Guid("c83601d0-fe9b-4b1c-adf5-1f2ef704e5ef") });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateCreated", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("c83601d0-fe9b-4b1c-adf5-1f2ef704e5ef"), 0, "5dee17e1-b532-49af-be0b-af4b7c607410", new DateTime(2022, 3, 27, 16, 47, 43, 117, DateTimeKind.Local).AddTicks(1604), "hoangthanh01022000@gmail.com", true, false, null, "hoangthanh01022000@gmail.com", "admin", "AQAAAAEAACcQAAAAEDvbg+rCEWP/tw5WfR7JfYr6eUT6xLksA7V4xfWIJmRVPrbLKFgmbhj7T3buwuq7mQ==", null, false, "", false, "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("25aad933-26f6-4343-a506-9fb41e90be1d"));

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("275dbac8-a1d2-484d-83f2-07718316271c"));

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("72ae53ec-a8cd-4a42-a6c5-827212e258f7"));

            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("25aad933-26f6-4343-a506-9fb41e90be1d"), new Guid("c83601d0-fe9b-4b1c-adf5-1f2ef704e5ef") });

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("c83601d0-fe9b-4b1c-adf5-1f2ef704e5ef"));

            migrationBuilder.DropColumn(
                name: "Performer",
                table: "Chats");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 3, 7, 16, 11, 0, 505, DateTimeKind.Local).AddTicks(7195),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 3, 27, 16, 47, 43, 81, DateTimeKind.Local).AddTicks(2721));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "CurriculumVitaes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 3, 7, 16, 11, 0, 500, DateTimeKind.Local).AddTicks(7310),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 3, 27, 16, 47, 43, 77, DateTimeKind.Local).AddTicks(1832));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Chats",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 3, 7, 16, 11, 0, 475, DateTimeKind.Local).AddTicks(1218),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 3, 27, 16, 47, 43, 54, DateTimeKind.Local).AddTicks(9173));

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5a5a6b22-c16c-488f-8c2c-36741b4ff55e"), "242521a2-7f80-4a55-9447-6bb014162cf7", "Company role", "company", "company" },
                    { new Guid("b2555e17-3b7d-48a8-8f5c-4becc36d0eed"), "ccf74cf7-3f7e-4db9-834b-de628d2122e2", "User role", "user", "user" },
                    { new Guid("c7b92a34-a345-4273-a0e8-b16032c08ace"), "95267d4e-fe62-4cf5-bd57-abeeaaaa9e32", "Admin role", "admin", "admin" }
                });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("c7b92a34-a345-4273-a0e8-b16032c08ace"), new Guid("8841e141-d060-4fd7-b55b-903e64dd757a") });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateCreated", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("8841e141-d060-4fd7-b55b-903e64dd757a"), 0, "fcd69333-8288-44d2-82e7-4bc2c91f56c6", new DateTime(2022, 3, 7, 16, 11, 0, 536, DateTimeKind.Local).AddTicks(6695), "hoangthanh01022000@gmail.com", true, false, null, "hoangthanh01022000@gmail.com", "admin", "AQAAAAEAACcQAAAAEBgJRl/lSyVu4QBw+aXVhL3muuflL/QlB5vJWLWb9BcIIG97gOGi+f1Pv8K7HB4JpQ==", null, false, "", false, "admin" });
        }
    }
}
