using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Npgsql;
using System;
using System.Collections.Generic;

namespace Example.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        public BookService Service;

        public BookController()
        {
            Service = new BookService();
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            try
            {
                var books = Service.GetAllBooks();
                return books.Count > 0 ? Ok(books) : NotFound("Nema dostupnih knjiga");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška pri dohvaćanju knjiga: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetBook(Guid id)

        {
            try
            {
                var book = Service.GetBookById(id);
                return book != null ? Ok(book) : NotFound($"Knjiga s ID-jem {id} nije pronađena");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška pri dohvaćanju knjige: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] Book newBook)
        {
            if (newBook == null)
            {
                return BadRequest("Podaci o knjizi nisu ispravni");
            }

            try
            {
                Service.AddBook(newBook);
                return Ok("Knjiga uspješno dodana");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška pri dodavanju knjige: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(Guid id, [FromBody] Book updatedBook)
        {
            if (updatedBook == null)
            {
                return BadRequest("Podaci za ažuriranje nisu ispravni");
            }

            try
            {
                var existingBook = Service.GetBookById(id);
                if (existingBook == null)
                {
                    return NotFound($"Knjiga s ID-jem {id} nije pronađena");
                }

                Service.UpdateBook(id, updatedBook);
                return Ok("Knjiga uspješno ažurirana");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška pri ažuriranju knjige: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(Guid id)
        {
            try
            {
                var existingBook = Service.GetBookById(id);
                if (existingBook == null)
                {
                    return NotFound($"Knjiga s ID-jem {id} nije pronađena");
                }

                Service.DeleteBook(id);
                return Ok("Knjiga uspješno izbrisana");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška pri brisanju knjige: {ex.Message}");
            }
        }
    }
}