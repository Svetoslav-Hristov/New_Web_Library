using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace New_Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class ForumAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topics_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Topics_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Posts_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, "Modern literary works", false, "Modern Literature" },
                    { 2, "Timeless classics", false, "Classical Literature" },
                    { 3, "Poems and verse", false, "Poetry" },
                    { 4, "Fantasy worlds and stories", false, "Fantasy" }
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "CategoryId", "CreatedOn", "IsDeleted", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 3, 7, 0, 8, 59, 757, DateTimeKind.Utc).AddTicks(1546), false, "Best modern novels 2026", new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 2, 2, new DateTime(2026, 3, 7, 0, 8, 59, 757, DateTimeKind.Utc).AddTicks(1551), false, "Top 10 classical books", new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 3, 3, new DateTime(2026, 3, 7, 0, 8, 59, 757, DateTimeKind.Utc).AddTicks(1553), false, "Favorite poets", new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 4, 4, new DateTime(2026, 3, 7, 0, 8, 59, 757, DateTimeKind.Utc).AddTicks(1554), false, "Epic fantasy series", new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Content", "CreatedOn", "IsDeleted", "Title", "TopicId", "UserId" },
                values: new object[,]
                {
                    { 1, "Let's discuss the best modern novels of 2026.", new DateTime(2026, 3, 7, 0, 8, 59, 756, DateTimeKind.Utc).AddTicks(9760), false, "Modern novel discussion", 1, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 2, "Share your favorite classical books.", new DateTime(2026, 3, 7, 0, 8, 59, 756, DateTimeKind.Utc).AddTicks(9771), false, "Classical books you love", 2, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 3, "Which poets inspire you?", new DateTime(2026, 3, 7, 0, 8, 59, 756, DateTimeKind.Utc).AddTicks(9773), false, "Poetry recommendations", 3, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 4, "Discuss your favorite fantasy series.", new DateTime(2026, 3, 7, 0, 8, 59, 756, DateTimeKind.Utc).AddTicks(9775), false, "Fantasy recommendations", 4, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PhoneNumber",
                table: "AspNetUsers",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TopicId",
                table: "Posts",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_CategoryId",
                table: "Topics",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_UserId",
                table: "Topics",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PhoneNumber",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("19c4ebff-4f5c-4504-8641-0dd4fb9f2218"),
                column: "ConcurrencyStamp",
                value: "ecbd63ed-9eb8-4d2d-9f41-7e40ca0143a8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30460549-2e0d-40c7-90ff-6f435900d186"),
                column: "ConcurrencyStamp",
                value: "01d75dd5-64b7-4666-aeb6-ac9907a0971b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("376b646e-7761-428b-b62b-21c58734fca7"),
                column: "ConcurrencyStamp",
                value: "e9018f28-ed55-45ea-9bac-6c439d0a62d8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5c80ef3a-faad-40f4-b245-45790594fe37"),
                column: "ConcurrencyStamp",
                value: "026ccefc-932f-4ef2-9ede-74a0d7a94788");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66757a02-9ffa-4c13-8070-6aeb39d5a570"),
                column: "ConcurrencyStamp",
                value: "2a6fc747-2e31-4aad-be84-7c36e153d933");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7023f574-e36a-4c31-b4a0-65bba3947199"),
                column: "ConcurrencyStamp",
                value: "3506b296-eeed-41d5-a779-8891e7df4baa");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("70d6692c-73ff-42fd-8992-1e175692b52f"),
                column: "ConcurrencyStamp",
                value: "6bf9e7b3-7701-403c-9b25-72bd0121f3d4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b97533fb-a904-4f0e-bacc-1dfd9f769122"),
                column: "ConcurrencyStamp",
                value: "0587382b-2097-44cf-82b6-53f76ee1fb12");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e6df1540-5bab-4126-b284-4a9af52c47cd"),
                column: "ConcurrencyStamp",
                value: "d6771b93-f9f3-40fd-894f-268f3653809e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f71797dc-7130-48d6-8f30-7d24d19bf347"),
                column: "ConcurrencyStamp",
                value: "a3a465e4-7609-40bb-96d7-6ae72029e983");
        }
    }
}
