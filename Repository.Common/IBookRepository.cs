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
        List<Book> GetAllBooks();

        Book GetBookById(Guid id);

        void AddBook(Book newBook);

        void UpdateBook(Guid id, Book updatedBook);

        void DeleteBook(Guid id);

        Book ReadBook(NpgsqlDataReader reader);
    }
}