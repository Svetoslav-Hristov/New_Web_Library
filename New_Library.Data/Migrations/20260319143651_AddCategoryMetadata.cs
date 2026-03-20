using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace New_Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteAt",
                table: "Topics",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Topics",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteAt",
                table: "Posts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Posts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteAt",
                table: "Comments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Comments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteAt",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("19c4ebff-4f5c-4504-8641-0dd4fb9f2218"),
                column: "ConcurrencyStamp",
                value: "efb34455-439e-42c4-9844-7c99474c6674");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30460549-2e0d-40c7-90ff-6f435900d186"),
                column: "ConcurrencyStamp",
                value: "6968cf06-0aa5-4285-94da-47f294748874");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("376b646e-7761-428b-b62b-21c58734fca7"),
                column: "ConcurrencyStamp",
                value: "0404f53a-58e9-4dc2-af52-65aa30e22030");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5c80ef3a-faad-40f4-b245-45790594fe37"),
                column: "ConcurrencyStamp",
                value: "abca0476-8842-4524-bb47-8edc1e408a9a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66757a02-9ffa-4c13-8070-6aeb39d5a570"),
                column: "ConcurrencyStamp",
                value: "cd18ec84-9a2d-4a2c-8ab4-968fe3b0f327");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7023f574-e36a-4c31-b4a0-65bba3947199"),
                column: "ConcurrencyStamp",
                value: "32c30950-ff6c-41d6-9245-c2ac0ca38be7");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("70d6692c-73ff-42fd-8992-1e175692b52f"),
                column: "ConcurrencyStamp",
                value: "a9c87cd7-c5d4-451f-933b-7e2d1f56f0cb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b97533fb-a904-4f0e-bacc-1dfd9f769122"),
                column: "ConcurrencyStamp",
                value: "0899c415-3512-4b09-a60b-0bffc93c9f40");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e6df1540-5bab-4126-b284-4a9af52c47cd"),
                column: "ConcurrencyStamp",
                value: "0b87b6a5-147b-4ef2-bc61-75b723760f31");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f71797dc-7130-48d6-8f30-7d24d19bf347"),
                column: "ConcurrencyStamp",
                value: "63a14c01-5fbd-4744-99a0-a4fd49c4a673");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DeleteAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DeleteAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DeleteAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DeleteAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DeleteAt", "Description", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 5, null, "Sci-fi adventures and futuristic stories", false, "Science Fiction", null },
                    { 6, null, "Stories set in historical periods", false, "Historical Fiction", null },
                    { 7, null, "Suspenseful and mysterious stories", false, "Mystery & Thriller", null },
                    { 8, null, "Informative and factual works", false, "Non-Fiction", null }
                });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DeleteAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 19, 14, 36, 49, 543, DateTimeKind.Utc).AddTicks(2156), null, null });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "DeleteAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 19, 14, 36, 49, 543, DateTimeKind.Utc).AddTicks(2175), null, null });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "DeleteAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 19, 14, 36, 49, 543, DateTimeKind.Utc).AddTicks(2178), null, null });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedOn", "DeleteAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 19, 14, 36, 49, 543, DateTimeKind.Utc).AddTicks(2180), null, null });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DeleteAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 19, 14, 36, 49, 543, DateTimeKind.Utc).AddTicks(4610), null, null });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "DeleteAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 19, 14, 36, 49, 543, DateTimeKind.Utc).AddTicks(4622), null, null });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "DeleteAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 19, 14, 36, 49, 543, DateTimeKind.Utc).AddTicks(4624), null, null });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedOn", "DeleteAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 19, 14, 36, 49, 543, DateTimeKind.Utc).AddTicks(4625), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DropColumn(
                name: "DeleteAt",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "DeleteAt",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "DeleteAt",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "DeleteAt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("19c4ebff-4f5c-4504-8641-0dd4fb9f2218"),
                column: "ConcurrencyStamp",
                value: "403479a2-0583-4dbd-aa8a-246caf55cabb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30460549-2e0d-40c7-90ff-6f435900d186"),
                column: "ConcurrencyStamp",
                value: "0bd9a0bc-4133-457a-b795-55b8b0ddd48c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("376b646e-7761-428b-b62b-21c58734fca7"),
                column: "ConcurrencyStamp",
                value: "9ad1336f-867e-4da4-9bf7-a772c345c4e0");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5c80ef3a-faad-40f4-b245-45790594fe37"),
                column: "ConcurrencyStamp",
                value: "bfe6a928-f691-46f5-981c-e5763d300a1d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66757a02-9ffa-4c13-8070-6aeb39d5a570"),
                column: "ConcurrencyStamp",
                value: "434064b8-fd69-442a-8bec-9c717d92895a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7023f574-e36a-4c31-b4a0-65bba3947199"),
                column: "ConcurrencyStamp",
                value: "f10fbc60-6e0f-4631-a6b6-898fead52dcf");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("70d6692c-73ff-42fd-8992-1e175692b52f"),
                column: "ConcurrencyStamp",
                value: "3c89d0df-3f6e-4ec2-8ae2-6e981b21f5c3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b97533fb-a904-4f0e-bacc-1dfd9f769122"),
                column: "ConcurrencyStamp",
                value: "5420847b-3d84-47bd-bb77-a1a8821cf650");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e6df1540-5bab-4126-b284-4a9af52c47cd"),
                column: "ConcurrencyStamp",
                value: "5b2b6554-9490-4ddf-bf2d-ca8c12fb0b60");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f71797dc-7130-48d6-8f30-7d24d19bf347"),
                column: "ConcurrencyStamp",
                value: "c45aa78a-d536-4de2-87c9-8f96eaf8e446");

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 3, 7, 0, 8, 59, 756, DateTimeKind.Utc).AddTicks(9760));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 3, 7, 0, 8, 59, 756, DateTimeKind.Utc).AddTicks(9771));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2026, 3, 7, 0, 8, 59, 756, DateTimeKind.Utc).AddTicks(9773));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2026, 3, 7, 0, 8, 59, 756, DateTimeKind.Utc).AddTicks(9775));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 3, 7, 0, 8, 59, 757, DateTimeKind.Utc).AddTicks(1546));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 3, 7, 0, 8, 59, 757, DateTimeKind.Utc).AddTicks(1551));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2026, 3, 7, 0, 8, 59, 757, DateTimeKind.Utc).AddTicks(1553));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2026, 3, 7, 0, 8, 59, 757, DateTimeKind.Utc).AddTicks(1554));
        }
    }
}
