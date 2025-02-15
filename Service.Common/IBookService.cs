﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Model;

namespace Service.Common
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBooksAsync(Sorting sorting, Paging paging, BookFilter bookFilter);

        Task<Book> GetBookByIdAsync(Guid id);

        Task AddBookAsync(Book newBook);

        Task UpdateBookAsync(Guid id, Book updatedBook);

        Task DeleteBookAsync(Guid id);
    }
}