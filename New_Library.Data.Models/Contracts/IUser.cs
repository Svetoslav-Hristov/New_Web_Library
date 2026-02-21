namespace New_Web_Library.Data.Models.Contracts
{
    public interface IUser
    {

        Guid Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        int Age { get; set; }
        string Address { get; set; }
        string PhoneNumber { get; set; }
        string Email { get; set; }
        bool IsBlocked { get; set; }
         

    }
}
