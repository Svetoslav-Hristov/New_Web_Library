using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace New_Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteUserAndUniqueEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("19c4ebff-4f5c-4504-8641-0dd4fb9f2218"),
                columns: new[] { "ConcurrencyStamp", "IsDeleted" },
                values: new object[] { "ecbd63ed-9eb8-4d2d-9f41-7e40ca0143a8", false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30460549-2e0d-40c7-90ff-6f435900d186"),
                columns: new[] { "ConcurrencyStamp", "IsDeleted" },
                values: new object[] { "01d75dd5-64b7-4666-aeb6-ac9907a0971b", false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("376b646e-7761-428b-b62b-21c58734fca7"),
                columns: new[] { "ConcurrencyStamp", "IsDeleted" },
                values: new object[] { "e9018f28-ed55-45ea-9bac-6c439d0a62d8", false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5c80ef3a-faad-40f4-b245-45790594fe37"),
                columns: new[] { "ConcurrencyStamp", "IsDeleted" },
                values: new object[] { "026ccefc-932f-4ef2-9ede-74a0d7a94788", false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66757a02-9ffa-4c13-8070-6aeb39d5a570"),
                columns: new[] { "ConcurrencyStamp", "IsDeleted" },
                values: new object[] { "2a6fc747-2e31-4aad-be84-7c36e153d933", false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7023f574-e36a-4c31-b4a0-65bba3947199"),
                columns: new[] { "ConcurrencyStamp", "IsDeleted" },
                values: new object[] { "3506b296-eeed-41d5-a779-8891e7df4baa", false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("70d6692c-73ff-42fd-8992-1e175692b52f"),
                columns: new[] { "ConcurrencyStamp", "IsDeleted" },
                values: new object[] { "6bf9e7b3-7701-403c-9b25-72bd0121f3d4", false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b97533fb-a904-4f0e-bacc-1dfd9f769122"),
                columns: new[] { "ConcurrencyStamp", "IsDeleted" },
                values: new object[] { "0587382b-2097-44cf-82b6-53f76ee1fb12", false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e6df1540-5bab-4126-b284-4a9af52c47cd"),
                columns: new[] { "ConcurrencyStamp", "IsDeleted" },
                values: new object[] { "d6771b93-f9f3-40fd-894f-268f3653809e", false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f71797dc-7130-48d6-8f30-7d24d19bf347"),
                columns: new[] { "ConcurrencyStamp", "IsDeleted" },
                values: new object[] { "a3a465e4-7609-40bb-96d7-6ae72029e983", false });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail",
                unique: true,
                filter: "[NormalizedEmail] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("19c4ebff-4f5c-4504-8641-0dd4fb9f2218"),
                column: "ConcurrencyStamp",
                value: "b03512e8-48ea-46ce-90a2-fa0b23f0f40f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30460549-2e0d-40c7-90ff-6f435900d186"),
                column: "ConcurrencyStamp",
                value: "9578811a-2a63-45bf-bf84-fb2e987fc74a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("376b646e-7761-428b-b62b-21c58734fca7"),
                column: "ConcurrencyStamp",
                value: "34d6d104-0d41-4395-9689-89eb1f1794f0");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5c80ef3a-faad-40f4-b245-45790594fe37"),
                column: "ConcurrencyStamp",
                value: "8ea6c40a-8b43-4045-920c-eb1da699f101");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66757a02-9ffa-4c13-8070-6aeb39d5a570"),
                column: "ConcurrencyStamp",
                value: "64156c43-aaf5-4ff8-9d51-ae5b4071e778");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7023f574-e36a-4c31-b4a0-65bba3947199"),
                column: "ConcurrencyStamp",
                value: "c4bb942c-97fb-4a1c-a42f-dd2044f0d4a7");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("70d6692c-73ff-42fd-8992-1e175692b52f"),
                column: "ConcurrencyStamp",
                value: "03cf7ee4-2d1b-497a-9ab8-81e245445e9a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b97533fb-a904-4f0e-bacc-1dfd9f769122"),
                column: "ConcurrencyStamp",
                value: "566b1f19-c770-4f8b-baf9-2e9d5ae0af31");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e6df1540-5bab-4126-b284-4a9af52c47cd"),
                column: "ConcurrencyStamp",
                value: "807e77de-db88-40d8-812a-ba5d2cbfdafd");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f71797dc-7130-48d6-8f30-7d24d19bf347"),
                column: "ConcurrencyStamp",
                value: "40818565-55a8-4534-9e47-22a51d5ee188");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");
        }
    }
}
