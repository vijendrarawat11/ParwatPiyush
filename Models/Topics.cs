﻿using System.ComponentModel.DataAnnotations;
namespace ParwatPiyushNewsPortal.Models
{
    public class Topics
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public List<News> News { get; set; } = new List<News>();
    }
}
