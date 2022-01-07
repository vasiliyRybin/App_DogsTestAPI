using System;

namespace DogsAppAPI.DB
{
    public class Dog
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int TailLength { get; set; }
        public int Weight { get; set; }
    }
}
