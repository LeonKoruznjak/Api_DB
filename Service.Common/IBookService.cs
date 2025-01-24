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
        public List<Book> GetAllBooks();

        public Book GetBookById(Guid id);

        public void AddBook(Book newBook);

        public void UpdateBook(Guid id, Book updatedBook);

        public void DeleteBook(Guid id);
    }
}