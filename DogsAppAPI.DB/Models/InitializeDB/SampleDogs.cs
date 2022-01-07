using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsAppAPI.DB.Models
{
    public static class SampleDogs
    {
        public static void Initialize(DogsDbContext context)
        {
            if (!context.Dogs.Any())
            {
                context.Dogs.AddRange(
                    new Dog
                    {
                        Name = "Neo",
                        Color = "Red & amber",
                        TailLength = 22,
                        Weight = 32
                    },
                    new Dog
                    {
                        Name = "Jessy",
                        Color = "Black & white",
                        TailLength = 7,
                        Weight = 14
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
