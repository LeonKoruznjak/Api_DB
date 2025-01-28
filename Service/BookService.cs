using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Repository;
using Service.Common;
using Repository.Common;
using Common;

namespace Service
{
    public class BookService : IBookService
    {
        private IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<Book>> GetAllBooksAsync(Sorting sorting, Paging paging)
        {
            return await _bookRepository.GetAllBooksAsync(sorting, paging);
        }

        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            return await _bookRepository.GetBookByIdAsync(id);
        }

        public async Task AddBookAsync(Book newBook)
        {
            if (newBook.Id == null || newBook.Id == Guid.Empty)
            {
                newBook.Id = Guid.NewGuid();
            }
            await _bookRepository.AddBookAsync(newBook);
        }

        public async Task UpdateBookAsync(Guid id, Book updatedBook)
        {
            await _bookRepository.UpdateBookAsync(id, updatedBook);
        }

        public async Task DeleteBookAsync(Guid id)
        {
            await _bookRepository.DeleteBookAsync(id);
        }
    }
}