using DogsAppAPI.DB;
using DogsAppAPI.Interfaces;
using System.Linq;
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

        public async Task<DogViewModel[]> GetAllDogs(PageParams pageParams)
        {
            AssignDefaultParamsToEmptyFields(pageParams);
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

                DogViewModel[] result = new DogViewModel[dogs.Count()];

                foreach (var dog in dogs)
                {
                    result[idx] = new DogViewModel { Name = dog.Name, Color = dog.Color, TailLength = dog.TailLength, Weight = dog.Weight };
                    idx++;
                }

                return result;
            }

            return null;
        }

        private static void AssignDefaultParamsToEmptyFields(PageParams pageParams)
        {
            if (string.IsNullOrWhiteSpace(pageParams.Attribute)) pageParams.Attribute = "name";
            if (string.IsNullOrWhiteSpace(pageParams.Order)) pageParams.Order = "asc";
        }
    }
}
