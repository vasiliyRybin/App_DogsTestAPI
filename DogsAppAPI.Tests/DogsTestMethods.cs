using DogsAppAPI.DB;
using DogsAppAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DogsAppAPI.Tests
{
    internal static class DogsTestMethods
    {
        public static IEnumerable<Dog> InitializeRepository()
        {
            Guid firstDog = Guid.Parse("F9F61F25-BA4E-4A35-3544-08D9D2356056");
            Guid secondDog = Guid.Parse("68F6A880-4743-43DB-3545-08D9D2356056");
            Guid thirdDog = Guid.Parse("584560BF-8446-48D6-8F6E-4BEAC9EFB646");

            return new List<Dog>
            {
            new Dog { ID = firstDog, Color = "Black", Name = "Phobos", TailLength = 10, Weight = 65 },
            new Dog { ID = secondDog, Color = "Amber", Name = "Jack", TailLength = 25, Weight = 30 },
            new Dog { ID = thirdDog, Color = "White", Name = "Vincent", TailLength = 45, Weight = 39 }
            };
        }

        public static List<DogExternalModel> GetOrderedListByAttribute(IEnumerable<Dog> dogs, PageParams pageParams)
        {
            var orderBy = typeof(Dog).GetProperties()
                    .Where(x => !x.Name.ToLower().StartsWith("id"))
                    .Where(x => !x.Name.ToLower().EndsWith("id"))
                    .FirstOrDefault(x => x.Name.ToLower() == pageParams.Attribute.ToLower());

            var propName = orderBy?.Name ?? "Name";

            List<DogExternalModel> orderedDogs = new();

            foreach (var item in dogs)
            {
                orderedDogs.Add(new DogExternalModel
                {
                    Color = item.Color,
                    Name = item.Name,
                    TailLength = item.TailLength,
                    Weight = item.Weight
                });
            }

            IOrderedEnumerable<DogExternalModel> preResult = default;

            if (pageParams.Order == "desc")
                preResult = orderedDogs
                    .OrderByDescending(x => typeof(DogExternalModel).GetProperty(propName).GetValue(x));
            else preResult = orderedDogs
                    .OrderBy(x => typeof(DogExternalModel).GetProperty(propName).GetValue(x));

            return preResult.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                    .Take(pageParams.PageSize)
                    .ToList();
        }

        public static bool ObjectsComparer(List<DogExternalModel> input, List<DogExternalModel> output)
        {
            int counter = 0;

            for (int i = 0; i < input.Count; i++)
            {
                if (
                    input[i].Color == output[i].Color &&
                    input[i].Name == output[i].Name &&
                    input[i].TailLength == output[i].TailLength &&
                    input[i].Weight == output[i].Weight
                  ) counter++;
            }

            if (counter == input.Count) return true;
            return false;
        }
    }
}
