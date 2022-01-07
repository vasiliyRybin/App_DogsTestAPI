﻿using Microsoft.EntityFrameworkCore;
using System;

namespace DogsAppAPI.DB
{
    public class DogViewModel
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int TailLength { get; set; }
        public int Weight { get; set; }
    }
}