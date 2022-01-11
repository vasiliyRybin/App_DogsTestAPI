using DogsAppAPI.DB;
using DogsAppAPI.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace DogsAppAPI.Tests
{
    public class DogsControllerTests
    {
        private static IEnumerable<Dog> InitializeRepository()
        {
            Guid firstDog = Guid.Parse("F9F61F25-BA4E-4A35-3544-08D9D2356056");
            Guid secondDog = Guid.Parse("68F6A880-4743-43DB-3545-08D9D2356056");
            Guid thirdDog = Guid.Parse("584560BF-8446-48D6-8F6E-4BEAC9EFB646");

            return new List<Dog>
            {
                new Dog{ ID = firstDog, Color = "Black", Name = "Phobos", TailLength = 10, Weight = 65},
                new Dog{ ID = secondDog, Color = "Amber", Name = "Jack", TailLength = 25, Weight = 30},
                new Dog{ ID = thirdDog, Color = "White", Name = "Vincent", TailLength = 45, Weight = 39},
            };
        }

        [Fact]
        public void GetAllDogs_CheckOutputType_InputCountEqualsOutputCount()
        {
            var data = InitializeRepository();
            Mock<IRepository<Dog>> DogRepo = new();
            //DogRepo.Setup(c => c.GetAsync().Result).Returns(data);
        }
    }
}
