namespace New_Web_Library.Data.Models.Contracts
{
    public interface IUser
    {
        
        string FirstName { get; set; }
        string LastName { get; set; }
        int Age { get; set; }
        string Address { get; set; }
        bool IsBlocked { get; set; }
        bool IsDeleted { get; set; }

    }
}
