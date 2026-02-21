using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using New_Web_Library.Data.Models;
using New_Web_Library.GCommon.Enums;

namespace New_Web_Library.Data.Configuration
{
    public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
    {

        private readonly Book[] initialBooks =
        {new Book
        {
            Id = Guid.Parse("b5cebf85-e61a-4e95-b688-a2b0e6893bed"),
            Title = "The Hobbit",
            Author = "J.R.R. Tolkien",
            Year=1937,
            CoverImageUrl="TheHobbit.jpg",
            Description="The Hobbit is a tale of high adventure, " +
            "undertaken by a company of dwarves in search of dragon-guarded gold." +
            " A reluctant partner in this perilous quest is Bilbo Baggins, a comfort-loving unambitious hobbit, " +
            "who surprises even himself by his resourcefulness and skill as a burglar.",
            Genre = Genre.Fantasy
        },
        new Book
        {
             Id = Guid.Parse("eeb61727-49d9-4b67-a836-1d3eacc4f08d"),
             Title = "1984",
             Author = "George Orwell",
             Year=1949,
             CoverImageUrl="1984.jpg",
             Description="Nineteen Eighty-Four: A Novel, often referred to as 1984," +
            " is a dystopian social science fiction novel by the English novelist " +
            "George Orwell (the pen name of Eric Arthur Blair). It was published on 8 June 1949 by Secker & Warburg as Orwell's " +
            "ninth and final book completed in his lifetime. Thematically, Nineteen Eighty-Four centres on the" +
            " consequences of totalitarianism, mass surveillance, and repressive regimentation of persons and behaviours within society." +
            " Orwell, himself a democratic socialist, modelled the authoritarian government in the novel after Stalinist Russia. More broadly," +
            " the novel examines the role of truth and facts within politics and the ways in which they are manipulated.",
             Genre = Genre.ScienceFiction
           
        },
        new Book
        {
             Id = Guid.Parse("cda5bf3e-e99c-4a9a-bc3c-889ec28b7031"),
             Title = "Brave New World",
             Author = "Aldous Huxley",
             Year=1932,
             CoverImageUrl = "BraveNewWorld.jpg",
             Description="Originally published in 1932, this outstanding work of literature is more crucial and relevant today than ever before." +
            " Cloning, feel-good drugs, antiaging programs, and total social control through politics, programming," +
            " and media -- has Aldous Huxley accurately predicted our future? With a storyteller's genius, he weaves these ethical controversies in" +
            " a compelling narrative that dawns in the year 632 AF (After Ford, the deity). When Lenina and Bernard visit a savage reservation," +
            " we experience how Utopia can destroy humanity. A powerful work of speculative fiction that has enthralled and terrified " +
            "readers for generations, " +"Brave New World is both a warning to be heeded and thought-provoking yet satisfying entertainment. - Container.",
             Genre = Genre.ScienceFiction
        },
        new Book
        {
             Id = Guid.Parse("f057357a-d690-48ce-a4cc-e1a9748cc63c"),
             Title = "Clean Code",
             Author = "Robert C. Martin",
             Year=2008,
             CoverImageUrl="CleanCode.jpg",
             Genre = Genre.Programming
        },
        new Book
        {
             Id = Guid.Parse("43f42ae2-7f5f-4944-871e-52d9fd057e6a"),
             Title = "The Pragmatic Programmer",
             Author = "Andrew Hunt",
             Year = 1999,
             CoverImageUrl="PragmaticProgramer.jpg",
             Description="Ward Cunningham Straight from the programming trenches, The Pragmatic Programmer cuts through the" +
            " increasing specialization and technicalities of modern software development to examine the core process--taking " +
            "a requirement and producing working, maintainable code that delights its users. It covers topics ranging from personal " +
            "responsibility and career development to architectural techniques for keeping your code flexible and easy to adapt and reuse. " +
            "Read this book, and you’ll learn how to Fight software rot; Avoid the trap of duplicating knowledge;" +
            " Write flexible, dynamic, and adaptable code; Avoid programming by coincidence; Bullet-proof your code with contracts, " +
            "assertions, and exceptions; Capture real requirements; Test ruthlessly and effectively; Delight your users;" +
            " Build teams of pragmatic programmers; and Make your developments more precise with automation. " +
            "Written as a series of self-contained sections and filled with entertaining anecdotes, thoughtful examples," +
            " and interesting analogies, The Pragmatic Programmer illustrates the best practices and major pitfalls of many different" +
            " aspects of software development. Whether you’re a new coder, an experienced program.",
             Genre = Genre.Programming
        },
        new Book
        {
             Id = Guid.Parse("5571dd6b-512c-4d64-8620-cf469800d93c"),
             Title = "The Da Vinci Code",
             Author = "Dan Brown",
             Year=2003,
             CoverImageUrl="DaVinciCode.jpg",
             Description="The Da Vinci Code is a 2003 mystery thriller novel by Dan Brown. It is Brown's second novel" +
            " to include the character Robert Langdon: the first was his 2000 novel Angels & Demons." +
            " The Da Vinci Code follows \"symbologist\" Robert Langdon and cryptologist Sophie Neveu after a murder in " +
            "the Louvre Museum in Paris causes them to become involved in a battle between the Priory of Sion and " +
            "Opus Dei over the possibility of Jesus Christ and Mary Magdalene having had a child together.",
             Genre = Genre.Thriller
        },
        new Book
        {
             Id = Guid.Parse("e2c70baa-d665-4ace-9409-6b053e41ed4f"),
             Title = "Sherlock Holmes",
             Author = "Arthur Conan Doyle",
             Year=1892,
             CoverImageUrl="SherlockHolmes.jpg",
             Genre = Genre.Mystery
        },
        new Book
        {
             Id = Guid.Parse("3fb3a2e4-5968-4eac-9838-eec211167561"),
             Title = "It",
             Author = "Stephen King",
             Year=1986,
             CoverImageUrl="It.jpg",
             Description="Derry: A small city in Maine, place as hauntingly familiar as your own hometown," +
            " only in Derry the haunting is real...\r\n\r\n" +
            "They were seven teenagers when they first" +
            " stumbled upon the horror. Now they are grown-up men and women who have gone out into the big world" +
            " to gain success and happiness. But none of them can withstand the force that has drawn them back to Derry " +
            "to face the nightmare without an end, and the evil without a name...",
             Genre = Genre.Horror
        },
        new Book
        {
             Id = Guid.Parse("65f62571-ee92-4e56-b6b9-dd7a8efe8e3f"),
             Title = "The Shining",
             Author = "Stephen King",
             Year=1977,
             CoverImageUrl="theShining.jpg",
             Description="The Shining is a 1977 horror novel by American author Stephen King. It is King's third published" +
            " novel and first hardback bestseller; its success firmly established King as a preeminent author in the horror genre." +
            " The setting and characters are influenced by King's personal experiences, including both his visit to The Stanley Hotel " +
            "in 1974 and his struggle with alcoholism. The book was followed by a sequel, Doctor Sleep, published in 2013.\r\n\r\n" +
            "The Shining centers on the life of Jack Torrance, a struggling writer and recovering alcoholic who accepts a position as the off-season" +
            " caretaker of the historic Overlook Hotel in the Colorado Rockies. His family accompanies him on this job, including his young son " +
            "Danny Torrance, who possesses \"the shining\", an array of psychic abilities that allow Danny to see the hotel's " +
            "horrific past. Soon, after a winter storm leaves them snowbound, the supernatural forces inhabiting the hotel influence Jack's sanity, leaving his wife and son in incredible danger.\r\n\r\n",
             Genre = Genre.Horror
        },
        new Book
        {
             Id = Guid.Parse("8c263c22-48fd-4ecd-9aa9-e01f2d8c0745"),
             Title = "The Alchemist",
             Author = "Paulo Coelho",
             Year=1988,
             CoverImageUrl="TheAlchemist.jpg",
             Description="The Alchemist details the journey of a young Andalusian shepherd boy named Santiago." +
            " Santiago, believing a recurring dream to be prophetic, decides to travel to the pyramids of Egypt to " +
            "find treasure. On the way, he encounters love, danger, opportunity and disaster. One of the significant characters " +
            "that he meets is an old king named Melchizedek who tells him that \"When you want something, all the universe conspires" +
            " in helping you to achieve it.\" This is the core philosophy and motif of the book.",
             Genre = Genre.Philosophy
        },
        new Book
        {
             Id = Guid.Parse("f315b770-aba6-4dd3-b9f0-c8b3d0dce787"),
             Title = "Sapiens",
             Author = "Yuval Noah Harari",
             Year=2011,
             CoverImageUrl="Sapiens.jpg",
             Genre = Genre.History
        },
        new Book
        {
             Id = Guid.Parse("4a8a88ba-79f6-44d6-8701-0c4d6bd7e412"),
             Title = "Homo Deus",
             Author = "Yuval Noah Harari",
             Year=2015,
             Genre = Genre.History
         },
        new Book
        {
             Id = Guid.Parse("ed7d0f41-9d87-4c33-a0c7-3620ed591fe3"),
             Title = "Steve Jobs",
             Author = "Walter Isaacson",
             Year=2011,
             CoverImageUrl="SteveJobs.jpg",
             Genre = Genre.Biography
        },
        new Book
        {
             Id = Guid.Parse("4f73e635-6290-4152-8c9b-cbcd4d09f496"),
             Title = "The Martian",
             Author = "Andy Weir",
             Year=2011,
             CoverImageUrl="Martian.jpg",
             Genre = Genre.ScienceFiction
        },
        new Book
        {
             Id = Guid.Parse("f15ea439-f2a1-40ac-8278-1056d7a75a52"),
             Title = "Dune",
             Author = "Frank Herbert",
             Year=1965,
             CoverImageUrl="Dune.jpg",
             Description="Set on the desert planet Arrakis, Dune is the story of the boy Paul Atreides," +
            " heir to a noble family tasked with ruling an inhospitable world where the only" +
            " thing of value is the \"spice\" melange, a drug capable of extending life and enhancing consciousness. " +
            "Coveted across the known universe, melange is a prize worth killing for...\r\n\r\n" +
            "When House Atreides is betrayed," +
            " the destruction of Paul's family will set the boy on a journey toward a destiny greater than" +
            " he could ever have imagined. And as he evolves into the mysterious man known as Muad'Dib, he will" +
            " bring to fruition humankind's most ancient and unattainable dream.\r\n\r\n" +
            "A stunning blend of adventure " +
            "and mysticism, environmentalism and politics, Dune won the first Nebula Award, shared the Hugo Award, and " +
            "formed the basis of what is undoubtedly the grandest epic in science fiction.",
             Genre = Genre.Fantasy
        },
        new Book
        {
             Id = Guid.Parse("1192f7d6-3197-4a88-87ce-a251dd1cf0b3"),
             Title = "The Name of the Wind",
             Author = "Patrick Rothfuss",
             Year=2007,
             CoverImageUrl="TheNameOfWind.jpg",
             Description="The Name of the Wind, also called The Kingkiller Chronicle: Day One, is a heroic fantasy novel " +
            "written by American author Patrick Rothfuss. It is the first book in the ongoing fantasy trilogy The Kingkiller Chronicle." +
            " It was published on March 27, 2007, by DAW Books, the novel has been hailed as a masterpiece of high fantasy.\r\n\r\n" +
            "The story begins the tale of Kvothe (pronounced \"quothe\"), a young man who becomes the most notorious magician his world" +
            " has ever known. Kvothe narrates his own journey, from his childhood in a troupe of traveling players to his years as a" +
            " near-feral orphan in a crime-ridden city, and his daring entrance into a prestigious and perilous school of magic.\r\n\r\n" +
            "Patrick Rothfuss's debut novel has been praised for its fresh and earthy originality, transporting readers into the mind of" +
            " a wizard and the world that shaped him. It explores the truth behind the legend of a hero and how one can become entangled" +
            " in their own mythology. Rothfuss's powerful storytelling and robust writing have earned him comparisons to renowned" +
            " fantasy authors such as Tad Williams, George R. R. Martin, and Robert Jordan.",
             Genre = Genre.Fantasy
        },
        new Book
        {
             Id = Guid.Parse("8be4237a-cadb-4f8c-bfd8-68eab7b7c64e"),
             Title = "To Kill a Mockingbird",
             Author = "Harper Lee",
             Year=1960,
             CoverImageUrl="ToKillMockingBird.jpg",
             Description="One of the best-loved stories of all time, To Kill a Mockingbird has been translated " +
            "into more than 40 languages, sold more than 30 million copies worldwide, served as the basis for an" +
            " enormously popular motion picture, and voted one of the best novels of the 20th century by librarians across" +
            " the United States. A gripping, heart-wrenching, and wholly remarkable tale of coming-of-age in a South poisoned by " +
            "virulent prejudice, it views a world of great beauty and savage inequities through the eyes of a young girl," +
            " as her father -- a crusading local lawyer -- risks everything to defend a black man unjustly accused of " +
            "a terrible crime.\r\n\r\n" +
            "Lawyer Atticus Finch defends Tom Robinson -- a black man charged with the rape" +
            " of a white girl. Writing through the young eyes of Finch's children Scout and Jem, Harper Lee explores" +
            " with rich humor and unswerving honesty the irrationality of adult attitudes toward race and class in small-town" +
            " Alabama during the mid-1930s Depression years. The conscience of a town steeped in prejudice, violence," +
            " and hypocrisy is pricked by the stamina and quiet heroism of one man's struggle for justice. But the weight" +
            " of history will only tolerate so much.",
             Genre = Genre.History
        },
        new Book
        {
             Id = Guid.Parse("3b57c8d7-a88e-4318-b5c4-0d1ab176c2e1"),
             Title = "Pride and Prejudice",
             Author = "Jane Austen",
             Year=1813,
             CoverImageUrl="Pride.jpg",
             Description="Pride and Prejudice is an 1813 novel of manners written by Jane Austen. " +
            "The novel follows the character development of Elizabeth Bennet, the dynamic protagonist " +
            "of the book who learns about the repercussions of hasty judgments and comes to appreciate the" +
            " difference between superficial goodness and actual goodness.\r\n\r\n" +
            "Mr. Bennet, owner of the Longbourn estate in Hertfordshire, has five daughters, but his property is " +
            "entailed and can only be passed to a male heir. His wife also lacks an inheritance, so his family faces becoming " +
            "very poor upon his death. Thus, it is imperative that at least one of the girls marry well " +
            "to support the others, which is a motivation that drives the plot.",
             Genre = Genre.Romance
        },
        new Book
        {
             Id = Guid.Parse("023fe09a-5ed3-4b7c-8eeb-f323f9e43cbf"),
             Title = "The Great Gatsby",
             Author = "F. Scott Fitzgerald",
             Year=1925,
             CoverImageUrl="TheGreatGatsby.jpg",
             Genre = Genre.Romance
        },
        new Book
        {
             Id = Guid.Parse("b6680038-dd42-4e75-9b24-ab7f2455393d"),
             Title = "Crime and Punishment",
             Author = "Fyodor Dostoevsky",
             Year=1866,
             CoverImageUrl="CrimeAndPunishment.jpg",
             Description="Crime and Punishment is a novel by Fyodor Dostoyevsky, first published in 1866." +
            " Translation to english by Constance Garnett.\r\n\r\n" +
            "In the peak heat of a St. Petersburg summer, an erstwhile university student, Rodion Romanovich Raskolnikov," +
            " commits a crime, bludgeoning a pawnbroker and her sister with an axe. What follows is a " +
            "psychological chess match between Raskolnikov and a wily detective that moves toward a form of redemption for" +
            " our antihero. Relentlessly philosophical and psychological, tackles freedom and strength, suffering and madness," +
            " illness, while asking if “great men” have license to forge their own moral codes.\r\n\r\n" +
            "Raskolnikov, a destitute and desperate former student, commits a random murder without remorse or regret," +
            " imagining himself to be a great man far above moral law. But as he embarks on a dangerous " +
            "cat-and-mouse game with a suspicious police investigator, his own conscience begins to torment him and he seeks" +
            " sympathy and redemption from Sonya, a downtrodden prostitute.",
             Genre = Genre.Philosophy
        },
        new Book
        {
             Id = Guid.Parse("7a183b2a-9af9-4412-9a2d-bcac7383e7ac"),
             Title = "The Catcher in the Rye",
             Author = "J.D. Salinger",
             Year=1945,
             CoverImageUrl="TheCatcherInTheRye.jpg",
             Genre = Genre.History
         },
        new Book
        {
             Id = Guid.Parse("d292d10e-8797-4b79-be46-150c747a58bf"),
             Title = "Dracula",
             Author = "Bram Stoker",
             Year=1897,
             CoverImageUrl="Dracula.jpg",
             Genre = Genre.Horror
        },
        new Book
        {
             Id = Guid.Parse("905974f8-f5b3-4eaf-a143-11b8345aca92"),
             Title = "Fahrenheit 451",
             Author = "Ray Bradbury",
             Year=1953,
             CoverImageUrl="Fahrenheit451.jpg",
             Genre = Genre.ScienceFiction
        },
        new Book
        {
             Id = Guid.Parse("7f7663d2-b426-4b01-af52-8d617a2fe553"),
             Title = "Refactoring",
             Author = "Martin Fowler",
             Year= 1999,
             CoverImageUrl="Refactoring.jpg",
             Genre = Genre.Programming
        }

        };


        public void Configure(EntityTypeBuilder<Book> entity)
        {
            entity.HasData(initialBooks);
        }
    }
}
