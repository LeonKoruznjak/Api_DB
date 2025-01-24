using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Model;

namespace Repository.Common
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBooks();

        Task<Book> GetBookById(Guid id);

        Task AddBook(Book newBook);

        Task UpdateBook(Guid id, Book updatedBook);

        Task DeleteBook(Guid id);

        Book ReadBook(NpgsqlDataReader reader);
    }
}