using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using New_Library.Data.Repository;
using New_Library.Data.Repository.Contracts;
using New_Library.Services.Core;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using New_Web_Library.Services.Core;
using New_Web_Library.Services.Core.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Web_Library.Services.Core.Tests
{
    [TestFixture]
    public class WelcomeServiceTests
    {
        [Test]
        public async Task GetLatestTitlesPreviewAsync_ShouldReturnTop5BooksOrderedByTitle()
        {
           
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryDbContext(options);

            context.Books.AddRange(new List<Book>
            {
                new Book { Id = new Guid(),Author= new Author{Name="Test Author" } ,Title = "Z Book", CoverImageUrl = "img1" },
                new Book { Id = new Guid(),Author=new Author{Name="Test Author" }, Title = "A Book", CoverImageUrl = "img2" },
                new Book { Id = new Guid(),Author=new Author{Name="Test Author" }, Title = "C Book", CoverImageUrl = "img3" },
                new Book { Id = new Guid(),Author=new Author{Name="Test Author" }, Title = "B Book", CoverImageUrl = null   },
                new Book { Id = new Guid(),Author=new Author{Name="Test Author" },Title = "D Book", CoverImageUrl = "img5" },
                new Book { Id = new Guid(),Author=new Author{Name="Test Author" }, Title = "E Book", CoverImageUrl = "img6" },
                new Book { Id = new Guid(),Author=new Author{Name="Test Author" }, Title = "F Book", CoverImageUrl = "img7" }
            });

            await context.SaveChangesAsync();

            var repo = new BookRepository(context);

            var envMock = new Mock<IWebHostEnvironment>();

            var systemRepoMock = new Mock<ISystemRepository>();

            var loggerMock = new Mock<ILogger<IBookService>>();

            var service = new WelcomeService(repo);



            var result = (await service.GetLatestTitlesPreviewAsync()).ToList();
            Assert.That(result.Count, Is.EqualTo(5));


        }

        [Test]

        public async Task GetLatestTitlesPreviewAsync_ShouldReturnEmpty_WhenNotValidBooks()
        {

            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryDbContext(options);

            context.Books.Add(new Book
            {
                Title = "No Image",
                Author = new Author 
                { 
                    Name = "Test Author" 
                },
                CoverImageUrl = null  
            });

            await context.SaveChangesAsync();

            var repo = new BookRepository(context);

            var envMock = new Mock<IWebHostEnvironment>();
            var systemRepoMock = new Mock<ISystemRepository>();
            var loggerMock = new Mock<ILogger<IBookService>>();

            var service = new WelcomeService(repo);

            var result = await service.GetLatestTitlesPreviewAsync();

            Assert.That(result.Count(), Is.EqualTo(0));

        }
        
        
        [Test]

        public async Task GetLatestTitlesPreviewAsync_ShouldBeOrderedByTiTle_Corectlly()
        {

            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryDbContext(options);

            context.Books.AddRange(new List<Book>
            {
                new Book { Id = new Guid(),Author=new Author{Name="Test Author" } ,Title = "Z Book", CoverImageUrl = "img1" },
                new Book { Id = new Guid(),Author=new Author{Name= "Test Author" }, Title = "A Book", CoverImageUrl = "img2" },
                new Book { Id = new Guid(),Author=new Author{Name="Test Author" }, Title = "C Book", CoverImageUrl = "img3" },
                new Book { Id = new Guid(),Author=new Author{Name="Test Author" }, Title = "B Book", CoverImageUrl = null   },
                new Book { Id = new Guid(),Author=new Author{Name="Test Author" } ,Title = "D Book", CoverImageUrl = "img5" },
                new Book { Id = new Guid(),Author=new Author{Name="Test Author" }, Title = "E Book", CoverImageUrl = "img6" },
                new Book { Id = new Guid(),Author=new Author{Name="Test Author" }, Title = "F Book", CoverImageUrl = "img7" }
            });

            await context.SaveChangesAsync();

            var repo = new BookRepository(context);

            var envMock = new Mock<IWebHostEnvironment>();

            var systemRepoMock = new Mock<ISystemRepository>();

            var loggerMock = new Mock<ILogger<IBookService>>();

            var service = new WelcomeService(repo);

            var result = (await service.GetLatestTitlesPreviewAsync());

            var titles = result.Select(b => b.Title).ToList();

            var sortedTitles = titles.OrderBy(t => t).ToList();


            Assert.AreEqual(sortedTitles, titles);



        }


    }
}
