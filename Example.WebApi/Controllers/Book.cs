﻿using System.ComponentModel.DataAnnotations;

namespace Example.WebApi.Controllers
{
    public class Book
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public Guid Id { get; set; }
        public int Quantity { get; set; }

       
      

        
        public Book(string title, string description, string author, int quantity)
        {
            Title = title;
            Description = description;
            Author = author;
            Id = Guid.NewGuid();
            Quantity = quantity;
        }


    }
}
