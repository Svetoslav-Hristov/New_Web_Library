using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using New_Web_Library.Data.Models;

namespace New_Web_Library.Data.Configuration
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {

        private readonly User[] initialUsers =
        {
         new User
         {
              Id = Guid.Parse("f71797dc-7130-48d6-8f30-7d24d19bf347"),
              FirstName = "Ivan",
              LastName = "Petrov",
              Age = 26,
              Address = "Sofia, Mladost 1",
              PhoneNumber = "+359888123456",
              Email = "ivan.petrov@library.bg",
              IsBlocked = false
         },
         new User
         {
              Id = Guid.Parse("19c4ebff-4f5c-4504-8641-0dd4fb9f2218"),
              FirstName = "Maria",
              LastName = "Georgieva",
              Age = 32,
              Address = "Sofia, Lozenets",
              PhoneNumber = "+359887654321",
              Email = "maria.georgieva@library.bg",
              IsBlocked = false
         },
         new User
         {
              Id = Guid.Parse("30460549-2e0d-40c7-90ff-6f435900d186"),
              FirstName = "Georgi",
              LastName = "Ivanov",
              Age = 41,
              Address = "Sofia, Nadezhda",
              PhoneNumber = "+359889777888",
              Email = "georgi.ivanov@library.bg",
              IsBlocked = true
         },
         new User
         {
              Id = Guid.Parse("e6df1540-5bab-4126-b284-4a9af52c47cd"),
              FirstName = "Elena",
              LastName = "Dimitrova",
              Age = 29,
              Address = "Sofia, Studentski Grad",
              PhoneNumber = "+359886333444",
              Email = "elena.dimitrova@library.bg",
              IsBlocked = false
         },
         new User
        {
              Id = Guid.Parse("b97533fb-a904-4f0e-bacc-1dfd9f769122"),
              FirstName = "Nikolay",
              LastName = "Stoyanov",
              Age = 35,
              Address = "Sofia, Krasno Selo",
              PhoneNumber = "+359885999000",
              Email = "nikolay.stoyanov@library.bg",
              IsBlocked = false
         },
         new User
         {
             Id = Guid.Parse("70d6692c-73ff-42fd-8992-1e175692b52f"),
             FirstName = "Petya",
             LastName = "Koleva",
             Age = 23,
             Address = "Sofia, Druzhba 2",
             PhoneNumber = "+359884222333",
             Email = "petya.koleva@library.bg",
             IsBlocked = false
         },
         new User
        {
             Id = Guid.Parse("376b646e-7761-428b-b62b-21c58734fca7"),
             FirstName = "Dimitar",
             LastName = "Hristov",
             Age = 46,
             Address = "Sofia, Obelya",
             PhoneNumber = "+359883111222",
             Email = "dimitar.hristov@library.bg",
             IsBlocked = true
        },
        new User
        {
             Id = Guid.Parse("5c80ef3a-faad-40f4-b245-45790594fe37"),
             FirstName = "Radostina",
             LastName = "Nikolova",
             Age = 30,
             Address = "Sofia, Geo Milev",
             PhoneNumber = "+359882444555",
             Email = "radostina.nikolova@library.bg",
             IsBlocked = false
         },
         new User
        {
             Id = Guid.Parse("66757a02-9ffa-4c13-8070-6aeb39d5a570"),
             FirstName = "Vladimir",
             LastName = "Angelov",
             Age = 34,
             Address = "Sofia, Lyulin 5",
             PhoneNumber = "+359881666777",
             Email = "vladimir.angelov@library.bg",
             IsBlocked = false
         },
         new User
        {
             Id = Guid.Parse("7023f574-e36a-4c31-b4a0-65bba3947199"),
             FirstName = "Desislava",
             LastName = "Popova",
             Age = 27,
             Address = "Sofia, Center",
             PhoneNumber = "+359880888999",
             Email = "desislava.popova@library.bg",
             IsBlocked = false
        }


        };


        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasData(initialUsers);
        }
    }
}
