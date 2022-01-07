using DogsAppAPI.DB;
using DogsAppAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<DogViewModel[]> GetAllDogs()
        {
            var dogs = await DogsRepo.Get();

            if (dogs.Any())
            {
                DogViewModel[] result = new DogViewModel[dogs.Count()];
                int idx = 0;

                foreach (var dog in dogs)
                {
                    result[idx] = new DogViewModel { Name = dog.Name, Color = dog.Color, TailLength = dog.TailLength, Weight = dog.Weight };
                    idx++;
                }

                return result;
            }

            return null;
        }
    }
}
