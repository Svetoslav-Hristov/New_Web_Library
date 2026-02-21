using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace New_Web_Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMIgration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Genre = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersBooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReservedOn = table.Column<DateOnly>(type: "date", nullable: true),
                    ReservationExpiresOn = table.Column<DateOnly>(type: "date", nullable: true),
                    PickUpDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ReturnDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersBooks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CoverImageUrl", "Description", "Genre", "Title", "Year" },
                values: new object[,]
                {
                    { new Guid("023fe09a-5ed3-4b7c-8eeb-f323f9e43cbf"), "F. Scott Fitzgerald", "TheGreatGatsby.jpg", null, 3, "The Great Gatsby", 1925 },
                    { new Guid("1192f7d6-3197-4a88-87ce-a251dd1cf0b3"), "Patrick Rothfuss", "TheNameOfWind.jpg", "The Name of the Wind, also called The Kingkiller Chronicle: Day One, is a heroic fantasy novel written by American author Patrick Rothfuss. It is the first book in the ongoing fantasy trilogy The Kingkiller Chronicle. It was published on March 27, 2007, by DAW Books, the novel has been hailed as a masterpiece of high fantasy.\r\n\r\nThe story begins the tale of Kvothe (pronounced \"quothe\"), a young man who becomes the most notorious magician his world has ever known. Kvothe narrates his own journey, from his childhood in a troupe of traveling players to his years as a near-feral orphan in a crime-ridden city, and his daring entrance into a prestigious and perilous school of magic.\r\n\r\nPatrick Rothfuss's debut novel has been praised for its fresh and earthy originality, transporting readers into the mind of a wizard and the world that shaped him. It explores the truth behind the legend of a hero and how one can become entangled in their own mythology. Rothfuss's powerful storytelling and robust writing have earned him comparisons to renowned fantasy authors such as Tad Williams, George R. R. Martin, and Robert Jordan.", 0, "The Name of the Wind", 2007 },
                    { new Guid("3b57c8d7-a88e-4318-b5c4-0d1ab176c2e1"), "Jane Austen", "Pride.jpg", "Pride and Prejudice is an 1813 novel of manners written by Jane Austen. The novel follows the character development of Elizabeth Bennet, the dynamic protagonist of the book who learns about the repercussions of hasty judgments and comes to appreciate the difference between superficial goodness and actual goodness.\r\n\r\nMr. Bennet, owner of the Longbourn estate in Hertfordshire, has five daughters, but his property is entailed and can only be passed to a male heir. His wife also lacks an inheritance, so his family faces becoming very poor upon his death. Thus, it is imperative that at least one of the girls marry well to support the others, which is a motivation that drives the plot.", 3, "Pride and Prejudice", 1813 },
                    { new Guid("3fb3a2e4-5968-4eac-9838-eec211167561"), "Stephen King", "It.jpg", "Derry: A small city in Maine, place as hauntingly familiar as your own hometown, only in Derry the haunting is real...\r\n\r\nThey were seven teenagers when they first stumbled upon the horror. Now they are grown-up men and women who have gone out into the big world to gain success and happiness. But none of them can withstand the force that has drawn them back to Derry to face the nightmare without an end, and the evil without a name...", 4, "It", 1986 },
                    { new Guid("43f42ae2-7f5f-4944-871e-52d9fd057e6a"), "Andrew Hunt", "PragmaticProgramer.jpg", "Ward Cunningham Straight from the programming trenches, The Pragmatic Programmer cuts through the increasing specialization and technicalities of modern software development to examine the core process--taking a requirement and producing working, maintainable code that delights its users. It covers topics ranging from personal responsibility and career development to architectural techniques for keeping your code flexible and easy to adapt and reuse. Read this book, and you’ll learn how to Fight software rot; Avoid the trap of duplicating knowledge; Write flexible, dynamic, and adaptable code; Avoid programming by coincidence; Bullet-proof your code with contracts, assertions, and exceptions; Capture real requirements; Test ruthlessly and effectively; Delight your users; Build teams of pragmatic programmers; and Make your developments more precise with automation. Written as a series of self-contained sections and filled with entertaining anecdotes, thoughtful examples, and interesting analogies, The Pragmatic Programmer illustrates the best practices and major pitfalls of many different aspects of software development. Whether you’re a new coder, an experienced program.", 7, "The Pragmatic Programmer", 1999 },
                    { new Guid("4a8a88ba-79f6-44d6-8701-0c4d6bd7e412"), "Yuval Noah Harari", null, null, 6, "Homo Deus", 2015 },
                    { new Guid("4f73e635-6290-4152-8c9b-cbcd4d09f496"), "Andy Weir", "Martian.jpg", null, 1, "The Martian", 2011 },
                    { new Guid("5571dd6b-512c-4d64-8620-cf469800d93c"), "Dan Brown", "DaVinciCode.jpg", "The Da Vinci Code is a 2003 mystery thriller novel by Dan Brown. It is Brown's second novel to include the character Robert Langdon: the first was his 2000 novel Angels & Demons. The Da Vinci Code follows \"symbologist\" Robert Langdon and cryptologist Sophie Neveu after a murder in the Louvre Museum in Paris causes them to become involved in a battle between the Priory of Sion and Opus Dei over the possibility of Jesus Christ and Mary Magdalene having had a child together.", 11, "The Da Vinci Code", 2003 },
                    { new Guid("65f62571-ee92-4e56-b6b9-dd7a8efe8e3f"), "Stephen King", "theShining.jpg", "The Shining is a 1977 horror novel by American author Stephen King. It is King's third published novel and first hardback bestseller; its success firmly established King as a preeminent author in the horror genre. The setting and characters are influenced by King's personal experiences, including both his visit to The Stanley Hotel in 1974 and his struggle with alcoholism. The book was followed by a sequel, Doctor Sleep, published in 2013.\r\n\r\nThe Shining centers on the life of Jack Torrance, a struggling writer and recovering alcoholic who accepts a position as the off-season caretaker of the historic Overlook Hotel in the Colorado Rockies. His family accompanies him on this job, including his young son Danny Torrance, who possesses \"the shining\", an array of psychic abilities that allow Danny to see the hotel's horrific past. Soon, after a winter storm leaves them snowbound, the supernatural forces inhabiting the hotel influence Jack's sanity, leaving his wife and son in incredible danger.\r\n\r\n", 4, "The Shining", 1977 },
                    { new Guid("7a183b2a-9af9-4412-9a2d-bcac7383e7ac"), "J.D. Salinger", "TheCatcherInTheRye.jpg", null, 6, "The Catcher in the Rye", 1945 },
                    { new Guid("7f7663d2-b426-4b01-af52-8d617a2fe553"), "Martin Fowler", "Refactoring.jpg", null, 7, "Refactoring", 1999 },
                    { new Guid("8be4237a-cadb-4f8c-bfd8-68eab7b7c64e"), "Harper Lee", "ToKillMockingBird.jpg", "One of the best-loved stories of all time, To Kill a Mockingbird has been translated into more than 40 languages, sold more than 30 million copies worldwide, served as the basis for an enormously popular motion picture, and voted one of the best novels of the 20th century by librarians across the United States. A gripping, heart-wrenching, and wholly remarkable tale of coming-of-age in a South poisoned by virulent prejudice, it views a world of great beauty and savage inequities through the eyes of a young girl, as her father -- a crusading local lawyer -- risks everything to defend a black man unjustly accused of a terrible crime.\r\n\r\nLawyer Atticus Finch defends Tom Robinson -- a black man charged with the rape of a white girl. Writing through the young eyes of Finch's children Scout and Jem, Harper Lee explores with rich humor and unswerving honesty the irrationality of adult attitudes toward race and class in small-town Alabama during the mid-1930s Depression years. The conscience of a town steeped in prejudice, violence, and hypocrisy is pricked by the stamina and quiet heroism of one man's struggle for justice. But the weight of history will only tolerate so much.", 6, "To Kill a Mockingbird", 1960 },
                    { new Guid("8c263c22-48fd-4ecd-9aa9-e01f2d8c0745"), "Paulo Coelho", "TheAlchemist.jpg", "The Alchemist details the journey of a young Andalusian shepherd boy named Santiago. Santiago, believing a recurring dream to be prophetic, decides to travel to the pyramids of Egypt to find treasure. On the way, he encounters love, danger, opportunity and disaster. One of the significant characters that he meets is an old king named Melchizedek who tells him that \"When you want something, all the universe conspires in helping you to achieve it.\" This is the core philosophy and motif of the book.", 8, "The Alchemist", 1988 },
                    { new Guid("905974f8-f5b3-4eaf-a143-11b8345aca92"), "Ray Bradbury", "Fahrenheit451.jpg", null, 1, "Fahrenheit 451", 1953 },
                    { new Guid("b5cebf85-e61a-4e95-b688-a2b0e6893bed"), "J.R.R. Tolkien", "TheHobbit.jpg", "The Hobbit is a tale of high adventure, undertaken by a company of dwarves in search of dragon-guarded gold. A reluctant partner in this perilous quest is Bilbo Baggins, a comfort-loving unambitious hobbit, who surprises even himself by his resourcefulness and skill as a burglar.", 0, "The Hobbit", 1937 },
                    { new Guid("b6680038-dd42-4e75-9b24-ab7f2455393d"), "Fyodor Dostoevsky", "CrimeAndPunishment.jpg", "Crime and Punishment is a novel by Fyodor Dostoyevsky, first published in 1866. Translation to english by Constance Garnett.\r\n\r\nIn the peak heat of a St. Petersburg summer, an erstwhile university student, Rodion Romanovich Raskolnikov, commits a crime, bludgeoning a pawnbroker and her sister with an axe. What follows is a psychological chess match between Raskolnikov and a wily detective that moves toward a form of redemption for our antihero. Relentlessly philosophical and psychological, tackles freedom and strength, suffering and madness, illness, while asking if “great men” have license to forge their own moral codes.\r\n\r\nRaskolnikov, a destitute and desperate former student, commits a random murder without remorse or regret, imagining himself to be a great man far above moral law. But as he embarks on a dangerous cat-and-mouse game with a suspicious police investigator, his own conscience begins to torment him and he seeks sympathy and redemption from Sonya, a downtrodden prostitute.", 8, "Crime and Punishment", 1866 },
                    { new Guid("cda5bf3e-e99c-4a9a-bc3c-889ec28b7031"), "Aldous Huxley", "BraveNewWorld.jpg", "Originally published in 1932, this outstanding work of literature is more crucial and relevant today than ever before. Cloning, feel-good drugs, antiaging programs, and total social control through politics, programming, and media -- has Aldous Huxley accurately predicted our future? With a storyteller's genius, he weaves these ethical controversies in a compelling narrative that dawns in the year 632 AF (After Ford, the deity). When Lenina and Bernard visit a savage reservation, we experience how Utopia can destroy humanity. A powerful work of speculative fiction that has enthralled and terrified readers for generations, Brave New World is both a warning to be heeded and thought-provoking yet satisfying entertainment. - Container.", 1, "Brave New World", 1932 },
                    { new Guid("d292d10e-8797-4b79-be46-150c747a58bf"), "Bram Stoker", "Dracula.jpg", null, 4, "Dracula", 1897 },
                    { new Guid("e2c70baa-d665-4ace-9409-6b053e41ed4f"), "Arthur Conan Doyle", "SherlockHolmes.jpg", null, 2, "Sherlock Holmes", 1892 },
                    { new Guid("ed7d0f41-9d87-4c33-a0c7-3620ed591fe3"), "Walter Isaacson", "SteveJobs.jpg", null, 5, "Steve Jobs", 2011 },
                    { new Guid("eeb61727-49d9-4b67-a836-1d3eacc4f08d"), "George Orwell", "1984.jpg", "Nineteen Eighty-Four: A Novel, often referred to as 1984, is a dystopian social science fiction novel by the English novelist George Orwell (the pen name of Eric Arthur Blair). It was published on 8 June 1949 by Secker & Warburg as Orwell's ninth and final book completed in his lifetime. Thematically, Nineteen Eighty-Four centres on the consequences of totalitarianism, mass surveillance, and repressive regimentation of persons and behaviours within society. Orwell, himself a democratic socialist, modelled the authoritarian government in the novel after Stalinist Russia. More broadly, the novel examines the role of truth and facts within politics and the ways in which they are manipulated.", 1, "1984", 1949 },
                    { new Guid("f057357a-d690-48ce-a4cc-e1a9748cc63c"), "Robert C. Martin", "CleanCode.jpg", null, 7, "Clean Code", 2008 },
                    { new Guid("f15ea439-f2a1-40ac-8278-1056d7a75a52"), "Frank Herbert", "Dune.jpg", "Set on the desert planet Arrakis, Dune is the story of the boy Paul Atreides, heir to a noble family tasked with ruling an inhospitable world where the only thing of value is the \"spice\" melange, a drug capable of extending life and enhancing consciousness. Coveted across the known universe, melange is a prize worth killing for...\r\n\r\nWhen House Atreides is betrayed, the destruction of Paul's family will set the boy on a journey toward a destiny greater than he could ever have imagined. And as he evolves into the mysterious man known as Muad'Dib, he will bring to fruition humankind's most ancient and unattainable dream.\r\n\r\nA stunning blend of adventure and mysticism, environmentalism and politics, Dune won the first Nebula Award, shared the Hugo Award, and formed the basis of what is undoubtedly the grandest epic in science fiction.", 0, "Dune", 1965 },
                    { new Guid("f315b770-aba6-4dd3-b9f0-c8b3d0dce787"), "Yuval Noah Harari", "Sapiens.jpg", null, 6, "Sapiens", 2011 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Age", "Email", "FirstName", "IsBlocked", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("19c4ebff-4f5c-4504-8641-0dd4fb9f2218"), "Sofia, Lozenets", 32, "maria.georgieva@library.bg", "Maria", false, "Georgieva", "+359887654321" },
                    { new Guid("30460549-2e0d-40c7-90ff-6f435900d186"), "Sofia, Nadezhda", 41, "georgi.ivanov@library.bg", "Georgi", true, "Ivanov", "+359889777888" },
                    { new Guid("376b646e-7761-428b-b62b-21c58734fca7"), "Sofia, Obelya", 46, "dimitar.hristov@library.bg", "Dimitar", true, "Hristov", "+359883111222" },
                    { new Guid("5c80ef3a-faad-40f4-b245-45790594fe37"), "Sofia, Geo Milev", 30, "radostina.nikolova@library.bg", "Radostina", false, "Nikolova", "+359882444555" },
                    { new Guid("66757a02-9ffa-4c13-8070-6aeb39d5a570"), "Sofia, Lyulin 5", 34, "vladimir.angelov@library.bg", "Vladimir", false, "Angelov", "+359881666777" },
                    { new Guid("7023f574-e36a-4c31-b4a0-65bba3947199"), "Sofia, Center", 27, "desislava.popova@library.bg", "Desislava", false, "Popova", "+359880888999" },
                    { new Guid("70d6692c-73ff-42fd-8992-1e175692b52f"), "Sofia, Druzhba 2", 23, "petya.koleva@library.bg", "Petya", false, "Koleva", "+359884222333" },
                    { new Guid("b97533fb-a904-4f0e-bacc-1dfd9f769122"), "Sofia, Krasno Selo", 35, "nikolay.stoyanov@library.bg", "Nikolay", false, "Stoyanov", "+359885999000" },
                    { new Guid("e6df1540-5bab-4126-b284-4a9af52c47cd"), "Sofia, Studentski Grad", 29, "elena.dimitrova@library.bg", "Elena", false, "Dimitrova", "+359886333444" },
                    { new Guid("f71797dc-7130-48d6-8f30-7d24d19bf347"), "Sofia, Mladost 1", 26, "ivan.petrov@library.bg", "Ivan", false, "Petrov", "+359888123456" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersBooks_BookId",
                table: "UsersBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersBooks_UserId",
                table: "UsersBooks",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersBooks");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
