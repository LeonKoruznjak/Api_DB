using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using static System.Reflection.Metadata.BlobBuilder;

namespace Example.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private static List<Book>? _books=new List<Book>();
        private const string CONNECTION_STRING = "Host=localhost:5432;" +
        "Username=postgres;" +
        "Password=leonpik7;" +
        "Database=LibraryDB";
      


        [HttpGet("test-connection")]
        public IActionResult TestConnection()
        {

            try
            {
                using ( var connection = new NpgsqlConnection(CONNECTION_STRING))
                {
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        return Ok("Uspješno spojen na bazu");
                    }
                    else
                    {
                        return StatusCode(500, "Veza s bazom nije uspjela");
                    }
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        

        [HttpGet]
        public ActionResult Get()
        {
            List<Book> books = new List<Book>();
            string commandText = $"SELECT * FROM Book";
            try
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))

                {
                    connection.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, connection))
                    {

                        

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                            while (reader.Read())
                            {
                                Book book = ReadBook(reader);
                                books.Add(book);
                               

                            }

                    }
                }
                return Ok(books);
            }
            
            catch (Exception ex)
            {
                return BadRequest($"Došlo je do greške: {ex.Message}");
            }

            return null;
        }


        private Book ReadBook(NpgsqlDataReader reader)
        {
            Guid id = Guid.Parse(reader["id"].ToString());
            string author = reader["author"] as string;
            string title = reader["title"] as string;
            string description = reader["description"] as string;
            int quantity =(int) reader["quantity"];

            Book book = new Book(title, description, author, (int)quantity, id);
           
            return book;
        }


        [HttpGet("{id}")]
        public  ActionResult Get(Guid id)
        {
            string commandText = $"SELECT * FROM Book WHERE Id = @id";
            try
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))
                   
                {
                     connection.Open();
                     using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, connection))
                    {
                        cmd.Parameters.AddWithValue("id", id);

                        using (NpgsqlDataReader reader =  cmd.ExecuteReader())
                            while ( reader.Read())
                            {
                                Book book = ReadBook(reader);
                                  return Ok(book);
                            }
                     
                    }
                }
                }
            catch (Exception ex)
            {
                return BadRequest($"Došlo je do greške: {ex.Message}");
            }
         
            return null;
        }


      




        [HttpPost]
        public  IActionResult AddBook(Book newBook)
        {
            string commandText = $"INSERT INTO Book (Id, Title, Author, Description, Quantity) " +
                                 $"VALUES (@id, @title, @author, @description, @quantity)";

            try
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))
                {
                     connection.OpenAsync();  

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

                return Ok("Knjiga uspješno dodana");
            }
            catch (Exception ex)
            {
                return BadRequest($"Došlo je do greške: {ex.Message}");
            }
        }



    


        [HttpPut("{id}")]
        public  IActionResult UpdateBook(Guid id,[FromBody] Book updatedBook)
        {


            var commandText = $"UPDATE Book SET Title = @title, Author=@author, Description=@description, Quantity=@quantity WHERE Id = @id";

            try
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))
                {
                     connection.Open();

                    using (var cmd = new NpgsqlCommand(commandText, connection))
                    {

                     
                        cmd.Parameters.AddWithValue("title", updatedBook.Title);
                        cmd.Parameters.AddWithValue("author", updatedBook.Author);
                        cmd.Parameters.AddWithValue("description", updatedBook.Description);
                        cmd.Parameters.AddWithValue("quantity", updatedBook.Quantity);
                      

                        cmd.Parameters.AddWithValue("Id", id); 


                         cmd.ExecuteNonQuery();
                    }
                }

                return Ok("Knjiga uspješno dodana");
            }
            catch (Exception ex)
            {
                return BadRequest($"Došlo je do greške: {ex.Message}");
            }
        }




        [HttpDelete("{id}")]
        public  IActionResult DeleteBook(Guid id)
        {


            var commandText = $"DELETE FROM Book WHERE Id = @id";

            try
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))
                {
                     connection.Open();

                    using (var cmd = new NpgsqlCommand(commandText, connection))
                    {



                        cmd.Parameters.AddWithValue("Id", id);


                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok("Knjiga uspješno izbrisana");
            }
            catch (Exception ex)
            {
                return BadRequest($"Došlo je do greške: {ex.Message}");
            }
        }

    }
}
