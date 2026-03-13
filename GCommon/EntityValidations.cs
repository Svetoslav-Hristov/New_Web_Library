

namespace New_Web_Library.GCommon
{
    public static class EntityValidations
    {
        public static class Admin
        {

            public const string adminRole = "Admin";
            public const string adminFirstName = "Jon";
            public const string adminLastName = "Smith";
            public const string adminEmail = "admin@library.com";
            public const int adminAge = 33;
            public const string adminAddress = "Some where in planet Earth ";
            public const string adminPhone = "555 000 555";
            public const string adminPassword = "Admin_123!";
        }


        public static class Users
        {
            public const int FirstNameUserMaxLength = 50;
            public const int FirstNameUserMinLength = 2;
            public const int LastNameUserMaxLength = 50;
            public const int LastNameUserMinLength = 2;
            public const int UserNameMaxLength = 256;
            public const int UserNameMinLength = 7;
            public const int AddressMaxLength = 200;
            public const int AddressMinLength = 10;
            public const int PhoneNumberMaxLength = 20;
            public const int PhoneNumberMinLength = 10;
            public const int EmailAddressMaxLength = 150;
            public const int EmailAddressMinLength = 8;
            public const int UserPasswordMinLength = 6;
            public const int UserPasswordMaxLength = 100;
            public const int UserMinAge = 5;
            public const int UserMaxAge = 120;

            public const int UserSearchCriteriaMax = 150;
            public const int UserSearchCriteriaMin = 8;

        }
        public static class Books
        {
            public const int TitleMaxLength = 100;
            public const int TitleMinLength = 1;
            public const int AuthorMaxLengthName = 100;
            public const int AuthorMinLengthName = 2;
            public const int URLMaxLength = 2048;
            public const int DescriptionMaxLength = 2000;



        }
        public static class UsersBooks
        {
            public const int ReservedExpiryPeriod = 3;
            public const int BorrowingExpiryPeriod = 25;


        }
        public static class Categories
        {
            public const int CategoryNameMaxLength = 50;
            public const int CategoryNameMinLength = 3;
            public const int DescriptionMaxLength = 500;
           

        }
        public static class Topics
        {
            public const int TopicTitleMaxLength = 150;
            public const int TopicTitleMinLength = 5;

        }
        public static class Posts
        {
            public const int PostTitleMaxLength = 120;
            public const int PostTitleMinLength = 5;
            public const int ContentMaxLength = 5000;
            public const int ContentMinLength = 10;
            public const int CommentLifeTime = 15;

        }
        


    }
}
