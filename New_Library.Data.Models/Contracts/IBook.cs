namespace New_Web_Library.Data.Models.Contracts
{
    public interface IBook
    {
        Guid Id { get; set; }
        string Title { get; set; }
        int Year { get; set; }
        string? CoverImageUrl { get; set; }
        string? Description { get; set; }
        string Author { get; set; }

    }
}
