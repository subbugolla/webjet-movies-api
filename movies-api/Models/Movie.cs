﻿using Newtonsoft.Json;

namespace MoviesApi.Models
{
    public class Movie
    {
        public string? Title { get; set; }
        public string? Year { get; set; }
        public string? ID { get; set; }
        public string? Type { get; set; }
        public string? Poster { get; set; }
    }
}
