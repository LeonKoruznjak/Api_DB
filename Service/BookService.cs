using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Repository;
using Service.Common;

namespace Service
{
    public class BookService : IBookService
    {
        protected BookRepository Repository;

        public BookService()
        {
            Repository = new BookRepository();
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await Repository.GetAllBooksAsync();
        }

        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            return await Repository.GetBookByIdAsync(id);
        }

        public async Task AddBookAsync(Book newBook)
        {
            await Repository.AddBookAsync(newBook);
        }

        public async Task UpdateBookAsync(Guid id, Book updatedBook)
        {
            await Repository.UpdateBookAsync(id, updatedBook);
        }

        public async Task DeleteBookAsync(Guid id)
        {
            await Repository.DeleteBookAsync(id);
        }
    }
}