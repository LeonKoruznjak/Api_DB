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

        public async Task<List<Book>> GetAllBooks()
        {
            return await Repository.GetAllBooks();
        }

        public async Task<Book> GetBookById(Guid id)
        {
            return await Repository.GetBookById(id);
        }

        public async Task AddBook(Book newBook)
        {
            await Repository.AddBook(newBook);
        }

        public async Task UpdateBook(Guid id, Book updatedBook)
        {
            await Repository.UpdateBook(id, updatedBook);
        }

        public async Task DeleteBook(Guid id)
        {
            await Repository.DeleteBook(id);
        }
    }
}