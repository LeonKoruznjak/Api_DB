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

        public List<Book> GetAllBooks()
        {
            return Repository.GetAllBooks();
        }

        public Book GetBookById(Guid id)
        {
            return Repository.GetBookById(id);
        }

        public void AddBook(Book newBook)
        {
            Repository.AddBook(newBook);
        }

        public void UpdateBook(Guid id, Book updatedBook)
        {
            Repository.UpdateBook(id, updatedBook);
        }

        public void DeleteBook(Guid id)
        {
            Repository.DeleteBook(id);
        }
    }
}