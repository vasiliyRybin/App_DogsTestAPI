using DogsAppAPI.DB;
using DogsAppAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DogsAppAPI.Web.Services
{
    public class DogsService
    {
        private IRepository<Dog> DogsRepo { get; }
        const int MAX_TAIL_LENGTH = 500;
        const int MAX_DOG_WEIGHT = 100;

        public DogsService(IRepository<Dog> repository)
        {
            DogsRepo = repository;
        }

        public async Task<List<DogExternalModel>> GetAllDogs(PageParams pageParams, bool test = false)
        {
            try
            {
                AssignDefaultParamsToEmptyInputFields(pageParams);
                var dogs = DogsRepo.Get();
                var orderBy = typeof(Dog).GetProperties()
                    .Where(x => !x.Name.ToLower().StartsWith("id"))
                    .Where(x => !x.Name.ToLower().EndsWith("id"))
                    .FirstOrDefault(x => x.Name.ToLower() == pageParams.Attribute.ToLower());

                var propName = orderBy?.Name ?? "Name";
                List<Dog> preResult = new();

                if (test)
                {
                    dogs = pageParams.Order == "desc" ?
                        dogs.OrderByDescending(x => x.GetType().GetProperty(propName).GetValue(x)) :
                        dogs.OrderBy(x => x.GetType().GetProperty(propName).GetValue(x));
                }
                else
                {
                    dogs = pageParams.Order == "desc" ?
                        dogs.OrderByDescending(x => EF.Property<object>(x, propName)) :
                        dogs.OrderBy(x => EF.Property<object>(x, propName));
                }

                dogs = dogs.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                        .Take(pageParams.PageSize);

                preResult = test ? dogs.ToList() : await dogs.ToListAsync();

                List<DogExternalModel> result = new();

                foreach (var dog in preResult)
                {
                    result.Add(new DogExternalModel
                    {
                        Name = dog.Name,
                        Color = dog.Color,
                        TailLength = dog.TailLength,
                        Weight = dog.Weight
                    });
                }

                return result;
            }
            catch (Exception)
            {
                return new List<DogExternalModel>();
            }
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

                var result = await DogsRepo.CreateAsync(dogResult);

                if (result) return dog;
            }

            return null;
        }

        private static bool CheckInput(DogExternalModel dog)
        {
            string pattern = @"^([A-Z]{1}[a-z]{1,10}\b)|^([А-Я]{1}[а-я]{1,10}\b)";
            if (string.IsNullOrWhiteSpace(dog.Name)) return false;
            if (string.IsNullOrWhiteSpace(dog.Color)) return false;
            if (dog.TailLength >= MAX_TAIL_LENGTH || dog.TailLength < 0) return false;
            if (dog.Weight <= 0 || dog.Weight >= MAX_DOG_WEIGHT) return false;
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
