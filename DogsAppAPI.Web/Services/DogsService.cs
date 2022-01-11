using DogsAppAPI.DB;
using DogsAppAPI.Interfaces;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DogsAppAPI.Web.Services
{
    public class DogsService
    {
        private IRepository<Dog> DogsRepo { get; }

        public DogsService(IRepository<Dog> repository)
        {
            DogsRepo = repository;
        }

        public async Task<DogExternalModel[]> GetAllDogs(PageParams pageParams)
        {
            AssignDefaultParamsToEmptyInputFields(pageParams);
            var dogs = await DogsRepo.Get();

            if (dogs.Any())
            {
                int idx = 0;
                var orderBy = typeof(Dog).GetProperties()
                    .FirstOrDefault(x => x.Name.ToLower() == pageParams.Attribute.ToLower());

                if (orderBy != null)
                {
                    var propName = orderBy.Name;

                    dogs = pageParams.Order == "desc" ?
                        dogs.OrderByDescending(x => x.GetType().GetProperty(propName).GetValue(x)) :
                        dogs.OrderBy(x => x.GetType().GetProperty(propName).GetValue(x));
                }
                else
                {
                    dogs = dogs = pageParams.Order == "desc" ?
                        dogs.OrderByDescending(x => x.Name) :
                        dogs.OrderBy(x => x.Name);
                }

                dogs = dogs.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                    .Take(pageParams.PageSize)
                    .ToList();

                DogExternalModel[] result = new DogExternalModel[dogs.Count()];

                foreach (var dog in dogs)
                {
                    result[idx] = new DogExternalModel { Name = dog.Name, Color = dog.Color, TailLength = dog.TailLength, Weight = dog.Weight };
                    idx++;
                }

                return result;
            }

            return null;
        }

        public async Task<DogExternalModel> CreateDog(DogExternalModel dog)
        {
            var check = CheckInput(dog);

            if (check)
            {
                Dog dogResult = new()
                {
                    ID = Guid.NewGuid(),
                    Name = dog.Name,
                    Color = dog.Color,
                    TailLength = dog.TailLength,
                    Weight = dog.Weight
                };

                var result = await DogsRepo.Create(dogResult);

                if (result) return dog;
            }

            return null;
        }

        private static bool CheckInput(DogExternalModel dog)
        {
            string pattern = @"^([A-Z]{1}[a-z]{1,10}\b)|^([А-Я]{1}[а-я]{1,10}\b)";
            if (string.IsNullOrWhiteSpace(dog.Name)) return false;
            if (string.IsNullOrWhiteSpace(dog.Color)) return false;
            if (dog.TailLength >= 500 || dog.TailLength < 0) return false;
            if (dog.Weight <= 0 || dog.Weight >= 100) return false;
            if (!Regex.Match(dog.Name, pattern).Success) return false;

            return true;
        }

        private static void AssignDefaultParamsToEmptyInputFields(PageParams pageParams)
        {
            if (string.IsNullOrWhiteSpace(pageParams.Attribute)) pageParams.Attribute = "name";
            if (string.IsNullOrWhiteSpace(pageParams.Order)) pageParams.Order = "asc";
        }
    }
}
