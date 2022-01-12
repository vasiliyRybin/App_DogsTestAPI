using DogsAppAPI.DB;
using DogsAppAPI.Interfaces;
using DogsAppAPI.Web;
using DogsAppAPI.Web.Services;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DogsAppAPI.Tests
{
    public class DogsControllerTests
    {

        [Fact]
        public void GetAllDogs_DefaultPageParams_OrderedByNameAsync()
        {
            var data = DogsTestMethods.InitializeRepository();
            var pageParams = new PageParams();
            var orderedDogs = DogsTestMethods.GetOrderedListByAttribute(data, pageParams);

            Mock<IRepository<Dog>> DogRepo = new();
            DogRepo.Setup(c => c.Get()).Returns(data.AsQueryable());
            var controller = new DogsService(DogRepo.Object);


            var result = controller.GetAllDogs(pageParams, true).Result;
            bool isEqual = DogsTestMethods.ObjectsComparer(orderedDogs, result);


            Assert.Equal(orderedDogs.Count, result.Count);
            Assert.True(isEqual);
        }

        [Fact]
        public void GetAllDogs_SortingDesc_IsEqualCollection()
        {
            var data = DogsTestMethods.InitializeRepository();
            var pageParams = new PageParams() { Order = "desc" };
            var orderedDogs = DogsTestMethods.GetOrderedListByAttribute(data, pageParams);

            Mock<IRepository<Dog>> DogRepo = new();
            DogRepo.Setup(c => c.Get()).Returns(data.AsQueryable());
            var controller = new DogsService(DogRepo.Object);

            var result = controller.GetAllDogs(pageParams, true).Result;
            bool isEqual = DogsTestMethods.ObjectsComparer(orderedDogs, result);

            Assert.Equal(orderedDogs.Count, result.Count);
            Assert.True(isEqual);
        }

        [Fact]
        public void GetAllDogs_FilledWrongParams_IsEmptyCollections()
        {
            var data = DogsTestMethods.InitializeRepository();
            var pageParams = new PageParams() { Order = "ASCENING", Attribute = "Calar", PageNumber = 100500, PageSize = 20000 };
            var orderedDogs = DogsTestMethods.GetOrderedListByAttribute(data, pageParams);

            Mock<IRepository<Dog>> DogRepo = new();
            DogRepo.Setup(c => c.Get()).Returns(data.AsQueryable());
            var controller = new DogsService(DogRepo.Object);

            var result = controller.GetAllDogs(pageParams, true).Result;
            bool isEqual = DogsTestMethods.ObjectsComparer(orderedDogs, result);

            Assert.Equal(orderedDogs.Count, result.Count);
            Assert.True(isEqual);
        }

        [Fact]
        public async Task CreateDog_CorrectInputParams_DogCreatedAsync()
        {
            var newDog = new DogExternalModel { Color = "Green", Name = "Orcdog", TailLength = 72, Weight = 78 };

            Mock<IRepository<Dog>> DogRepo = new();
            DogRepo.Setup(c => c.CreateAsync(It.IsAny<Dog>()).Result).Returns(true);
            var controller = new DogsService(DogRepo.Object);

            var result = await controller.CreateDog(newDog);

            Assert.NotNull(result);
            Assert.IsType<DogExternalModel>(result);
        }

        [Fact]
        public async Task CreateDog_WrongWeight_NotCreated()
        {
            var newDog = new DogExternalModel { Color = "Blue", Name = "BadDog", TailLength = 150, Weight = -20 };

            Mock<IRepository<Dog>> DogRepo = new();
            DogRepo.Setup(c => c.CreateAsync(It.IsAny<Dog>()).Result).Returns(false);
            var controller = new DogsService(DogRepo.Object);

            var result = await controller.CreateDog(newDog);

            Assert.Null(result);
        }
    }
}
