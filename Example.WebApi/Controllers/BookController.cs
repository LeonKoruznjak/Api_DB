using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Npgsql;
using System;
using System.Collections.Generic;
using Service.Common;
using Common;

namespace Example.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        public IBookService _service;

        public BookController(IBookService service)
        {
            _service = service;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetBooksAsync()
        //{
        //    try
        //    {
        //        return books.Count > 0 ? Ok(books) : NotFound("Nema dostupnih knjiga");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Greška pri dohvaćanju knjiga: {ex.Message}");
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> GetBooksAsync([FromQuery] string orderBy = "Id", [FromQuery] string sortOrder = "asc", [FromQuery] int pageNumber = 1, [FromQuery] int rpp = 10,
            [FromQuery] string? author = null, [FromQuery] string? title = null, [FromQuery] int? quantity = null)
        {
            try
            {
                var bookFilter = new BookFilter
                {
                    Author = author,
                    Title = title,
                    Quantity = quantity,
                };

                var sorting = new Sorting
                {
                    OrderBy = orderBy,
                    SortOrder = sortOrder
                };

                var paging = new Paging
                {
                    PageNumber = pageNumber,
                    Rpp = rpp
                };

                var books = await _service.GetAllBooksAsync(sorting, paging, bookFilter);

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
                var book = await _service.GetBookByIdAsync(id);
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
                await _service.AddBookAsync(newBook);
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
                var existingBook = await _service.GetBookByIdAsync(id);
                if (existingBook == null)
                {
                    return NotFound($"Knjiga s ID-jem {id} nije pronađena");
                }

                await _service.UpdateBookAsync(id, updatedBook);
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
                var existingBook = await _service.GetBookByIdAsync(id);
                if (existingBook == null)
                {
                    return NotFound($"Knjiga s ID-jem {id} nije pronađena");
                }

                await _service.DeleteBookAsync(id);
                return Ok("Knjiga uspješno izbrisana");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška pri brisanju knjige: {ex.Message}");
            }
        }
    }
}