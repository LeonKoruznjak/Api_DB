using Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Repository.Common;
using Common;
using System.Text;

namespace Repository
{
    public class BookRepository : IBookRepository
    {
        private const string CONNECTION_STRING = "Host=localhost:5432;Username=postgres;Password=leonpik7;Database=LibraryDB";

        public async Task<List<Book>> GetAllBooksAsync(Sorting sorting, Paging paging)
        {
            var books = new List<Book>();

            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    var commandText = new StringBuilder();
                    commandText.Append("SELECT * FROM Book ");
                    if (sorting != null)
                    {
                        commandText.Append($" ORDER BY {sorting.OrderBy} {sorting.SortOrder.ToUpper()} ");
                    }
                    if (paging != null)
                    {
                        commandText.Append($"OFFSET @OFFSET FETCH NEXT @ROWS ROWS ONLY;");
                        cmd.Parameters.AddWithValue("@OFFSET", paging.PageNumber == 1 ? 0 : (paging.PageNumber - 1) * paging.Rpp); //bila je greska u +1, ako imamo 4 itema i zelimo dva po stranici, 2-1=1*2=2+1=3 i ofset je 3 onda znaci da prva 3 ne gledamo nego gledamo od 4. koji je ujedno i zadnji i zato je samo jednog izbacivao?
                        cmd.Parameters.AddWithValue("@ROWS", paging.Rpp);
                    }
                    cmd.CommandText = commandText.ToString();
                    connection.Open();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            books.Add(ReadBook(reader));
                        }
                    }
                }
            }

            return books;
        }

        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            string commandText = "SELECT * FROM Book WHERE Id = @id";

            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(commandText, connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return ReadBook(reader);
                        }
                    }
                }
            }
            return null;
        }

        public async Task AddBookAsync(Book newBook)
        {
            string commandText = "INSERT INTO Book (Id, Title, Author, Description, Quantity) VALUES (@id, @title, @author, @description, @quantity)";

            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(commandText, connection))
                {
                    cmd.Parameters.AddWithValue("id", newBook.Id);
                    cmd.Parameters.AddWithValue("title", newBook.Title);
                    cmd.Parameters.AddWithValue("author", newBook.Author);
                    cmd.Parameters.AddWithValue("description", newBook.Description);
                    cmd.Parameters.AddWithValue("quantity", newBook.Quantity);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateBookAsync(Guid id, Book updatedBook)
        {
            string commandText = "UPDATE Book SET Title = @title, Author = @author, Description = @description, Quantity = @quantity WHERE Id = @id";

            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(commandText, connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("title", updatedBook.Title);
                    cmd.Parameters.AddWithValue("author", updatedBook.Author);
                    cmd.Parameters.AddWithValue("description", updatedBook.Description);
                    cmd.Parameters.AddWithValue("quantity", updatedBook.Quantity);
                    await cmd.ExecuteReaderAsync();
                }
            }
        }

        public async Task DeleteBookAsync(Guid id)
        {
            string commandText = "DELETE FROM Book WHERE Id = @id";

            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(commandText, connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public Book ReadBook(NpgsqlDataReader reader)
        {
            return new Book(
                reader["title"] as string,
                reader["description"] as string,
                reader["author"] as string,
                (int)reader["quantity"],
                Guid.Parse(reader["id"].ToString())
            );
        }
    }
}