﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Model;
using Common;

namespace Repository.Common
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBooksAsync(Sorting sorting, Paging paging, BookFilter bookFilter);

        Task<Book> GetBookByIdAsync(Guid id);

        Task AddBookAsync(Book newBook);

        Task UpdateBookAsync(Guid id, Book updatedBook);

        Task DeleteBookAsync(Guid id);

        Book ReadBook(NpgsqlDataReader reader);
    }
}