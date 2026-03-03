

namespace New_Web_Library.GCommon
{
    public static class EntityValidations
    {
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

    }
}
