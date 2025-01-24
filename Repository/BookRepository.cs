using Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Repository.Common;

namespace Repository
{
    public class BookRepository : IBookRepository
    {
        private const string CONNECTION_STRING = "Host=localhost:5432;Username=postgres;Password=leonpik7;Database=LibraryDB";

        public List<Book> GetAllBooks()
        {
            var books = new List<Book>();
            string commandText = "SELECT * FROM Book";

            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(commandText, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(ReadBook(reader));
                        }
                    }
                }
            }
            return books;
        }

        public Book GetBookById(Guid id)
        {
            string commandText = "SELECT * FROM Book WHERE Id = @id";

            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(commandText, connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ReadBook(reader);
                        }
                    }
                }
            }
            return null;
        }

        public void AddBook(Book newBook)
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
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateBook(Guid id, Book updatedBook)
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
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteBook(Guid id)
        {
            string commandText = "DELETE FROM Book WHERE Id = @id";

            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(commandText, connection))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
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