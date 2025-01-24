using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Service.Common
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBooks();

        Task<Book> GetBookById(Guid id);

        Task AddBook(Book newBook);

        Task UpdateBook(Guid id, Book updatedBook);

        Task DeleteBook(Guid id);
    }
}