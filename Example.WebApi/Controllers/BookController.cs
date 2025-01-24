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
        public async Task<IActionResult> GetBooksAsync()
        {
            try
            {
                var books = await Service.GetAllBooksAsync();
                return books.Count > 0 ? Ok(books) : NotFound("Nema dostupnih knjiga");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška pri dohvaćanju knjiga: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(Guid id)

        {
            try
            {
                var book = await Service.GetBookByIdAsync(id);
                return book != null ? Ok(book) : NotFound($"Knjiga s ID-jem {id} nije pronađena");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška pri dohvaćanju knjige: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBookAsync(Book newBook)
        {
            if (newBook == null)
            {
                return BadRequest("Podaci o knjizi nisu ispravni");
            }

            try
            {
                await Service.AddBookAsync(newBook);
                return Ok("Knjiga uspješno dodana");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška pri dodavanju knjige: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookAsync(Guid id, [FromBody] Book updatedBook)
        {
            if (updatedBook == null)
            {
                return BadRequest("Podaci za ažuriranje nisu ispravni");
            }

            try
            {
                var existingBook = await Service.GetBookByIdAsync(id);
                if (existingBook == null)
                {
                    return NotFound($"Knjiga s ID-jem {id} nije pronađena");
                }

                await Service.UpdateBookAsync(id, updatedBook);
                return Ok("Knjiga uspješno ažurirana");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška pri ažuriranju knjige: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookAsync(Guid id)
        {
            try
            {
                var existingBook = await Service.GetBookByIdAsync(id);
                if (existingBook == null)
                {
                    return NotFound($"Knjiga s ID-jem {id} nije pronađena");
                }

                await Service.DeleteBookAsync(id);
                return Ok("Knjiga uspješno izbrisana");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška pri brisanju knjige: {ex.Message}");
            }
        }
    }
}