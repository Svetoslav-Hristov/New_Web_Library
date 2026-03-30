using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace New_Web_Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("19c4ebff-4f5c-4504-8641-0dd4fb9f2218"),
                column: "ConcurrencyStamp",
                value: "915b93fa-a251-426c-9bbd-a3bb9df23186");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30460549-2e0d-40c7-90ff-6f435900d186"),
                column: "ConcurrencyStamp",
                value: "1b8c1f13-9094-42c7-ad20-e4d80fe200aa");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("376b646e-7761-428b-b62b-21c58734fca7"),
                column: "ConcurrencyStamp",
                value: "78e8d2e8-fa41-4cd4-a549-1592ebc291a5");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5c80ef3a-faad-40f4-b245-45790594fe37"),
                column: "ConcurrencyStamp",
                value: "e65daacf-5414-428c-9ef7-393f504da0d3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66757a02-9ffa-4c13-8070-6aeb39d5a570"),
                column: "ConcurrencyStamp",
                value: "ce57b2f2-0d64-42ed-80d2-f44685e0b146");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7023f574-e36a-4c31-b4a0-65bba3947199"),
                column: "ConcurrencyStamp",
                value: "18c25e33-017f-4a10-afc7-b9accec430c3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("70d6692c-73ff-42fd-8992-1e175692b52f"),
                column: "ConcurrencyStamp",
                value: "634d6f92-340e-4d86-9963-28c3b7995821");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b97533fb-a904-4f0e-bacc-1dfd9f769122"),
                column: "ConcurrencyStamp",
                value: "40d550fc-d0f1-4d9b-9abc-c028559b9656");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e6df1540-5bab-4126-b284-4a9af52c47cd"),
                column: "ConcurrencyStamp",
                value: "88d81210-c809-40e6-8cfc-55b7e0d38dcf");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f71797dc-7130-48d6-8f30-7d24d19bf347"),
                column: "ConcurrencyStamp",
                value: "60c7dfc7-f62f-4072-926b-8a6622a4dbf7");

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "CategoryId", "CreatedOn", "DeleteAt", "IsDeleted", "Title", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(2748), null, false, "Best modern novels 2026", null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 2, 2, new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(2757), null, false, "Top 10 classical books", null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 3, 3, new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(2759), null, false, "Favorite poets", null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 4, 4, new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(2762), null, false, "Epic fantasy series", null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 5, 1, new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(2764), null, false, "Modern short stories", null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 6, 1, new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(2767), null, false, "Contemporary novels discussion", null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 7, 2, new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(2770), null, false, "Shakespeare's works", null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 8, 2, new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(2772), null, false, "Greek and Roman classics", null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 9, 5, new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(2775), null, false, "Future tech and space exploration", null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 10, 6, new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(2778), null, false, "World War II novels", null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 11, 7, new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(2780), null, false, "Detective series discussion", null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Content", "CreatedOn", "DeleteAt", "IsDeleted", "Title", "TopicId", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, "Let's discuss the best modern novels of 2026.", new DateTime(2026, 3, 30, 17, 41, 15, 525, DateTimeKind.Utc).AddTicks(9880), null, false, "Modern novel discussion", 1, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 2, "Share your favorite classical books.", new DateTime(2026, 3, 30, 17, 41, 15, 525, DateTimeKind.Utc).AddTicks(9895), null, false, "Classical books you love", 2, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 3, "Which poets inspire you?", new DateTime(2026, 3, 30, 17, 41, 15, 525, DateTimeKind.Utc).AddTicks(9897), null, false, "Poetry recommendations", 3, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 4, "Discuss your favorite fantasy series.", new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(13), null, false, "Fantasy recommendations", 4, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 5, "Which modern short stories are worth reading?", new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(16), null, false, "Modern short story debate", 1, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 6, "Share insights on contemporary novels you've read recently.", new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(18), null, false, "Contemporary novels insights", 1, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 7, "Let's explore the themes in classic literature.", new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(20), null, false, "Exploring classic literature", 2, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 8, "Who are your favorite classic authors and why?", new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(22), null, false, "Favorite classic authors", 2, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "CreatedOn", "DeleteAt", "IsDeleted", "PostId", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, "I think 2026 has some really strong releases already.", new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(9445), null, false, 1, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 2, "Any recommendations for modern drama novels?", new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(9456), null, false, 1, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 3, "I've recently read a great psychological novel, highly recommend!", new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(9459), null, false, 1, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 4, "Modern literature is getting more diverse, which is awesome.", new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(9622), null, false, 1, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 5, "Do you prefer physical books or eBooks?", new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(9625), null, false, 1, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 6, "I feel like modern novels focus more on characters than plot.", new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(9628), null, false, 1, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 7, "Can someone suggest a good mystery novel from 2026?", new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(9631), null, false, 1, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 8, "Audiobooks are also becoming very popular lately.", new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(9634), null, false, 1, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 9, "I love how modern authors experiment with storytelling.", new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(9636), null, false, 1, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") },
                    { 10, "Looking forward to your suggestions!", new DateTime(2026, 3, 30, 17, 41, 15, 526, DateTimeKind.Utc).AddTicks(9638), null, false, 1, null, new Guid("8fd866b1-9516-429a-3aaf-08de7ab2efc7") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("19c4ebff-4f5c-4504-8641-0dd4fb9f2218"),
                column: "ConcurrencyStamp",
                value: "ce501836-8b32-4eee-a585-f3032a66d18e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30460549-2e0d-40c7-90ff-6f435900d186"),
                column: "ConcurrencyStamp",
                value: "5ba41430-2d44-4a1a-8adf-afa061203715");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("376b646e-7761-428b-b62b-21c58734fca7"),
                column: "ConcurrencyStamp",
                value: "e5d5dd0a-f177-4f5d-83c9-cea8b728cb38");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5c80ef3a-faad-40f4-b245-45790594fe37"),
                column: "ConcurrencyStamp",
                value: "39cfe6ac-bdf8-4110-82d8-7e8cb51b6d6b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66757a02-9ffa-4c13-8070-6aeb39d5a570"),
                column: "ConcurrencyStamp",
                value: "aedfac48-dbbf-49cc-a6f3-57019b353821");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7023f574-e36a-4c31-b4a0-65bba3947199"),
                column: "ConcurrencyStamp",
                value: "3278c7bb-8db9-41ed-b071-ef4b6144daa5");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("70d6692c-73ff-42fd-8992-1e175692b52f"),
                column: "ConcurrencyStamp",
                value: "3c2b8f54-de5d-43d3-8dd7-fc7a3130aab7");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b97533fb-a904-4f0e-bacc-1dfd9f769122"),
                column: "ConcurrencyStamp",
                value: "c2196b70-cbd0-4c64-a8e6-ee00d83d5af4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e6df1540-5bab-4126-b284-4a9af52c47cd"),
                column: "ConcurrencyStamp",
                value: "0960fedc-a600-4cda-88ba-5b6cf882d30e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f71797dc-7130-48d6-8f30-7d24d19bf347"),
                column: "ConcurrencyStamp",
                value: "f269957b-5691-4753-a68a-fe93ad96bd6b");
        }
    }
}
