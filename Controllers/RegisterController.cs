using Microsoft.AspNetCore.Mvc;
using System;
using BackendAPI.Model;
using BackendAPI.SqlConnection;
using Microsoft.EntityFrameworkCore; // Import Entity Framework Core
using BackendAPI;
using MySqlConnector;

namespace BackendAPI.Controllers
{
    [Route("/users")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly Connect _SQLconnection;

        public RegisterController()
        {
            _SQLconnection = new Connect();
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserModel userModel)
        {
            try
            {
                // Validate and process the user data here

                // Open the database connection
                _SQLconnection.EnsureConnectionOpen();

                // Create a SQL command to insert the user data into the database
                using var cmd = new MySqlCommand(
                    "INSERT INTO users (username, password, bio) VALUES (@username, @password, @bio)",
                    _SQLconnection._connection
                );

                // Add parameters to the SQL command
                cmd.Parameters.AddWithValue("@username", userModel.username);
                cmd.Parameters.AddWithValue("@password", userModel.password);
                cmd.Parameters.AddWithValue("@bio", userModel.bio);

                // Execute the SQL command
                cmd.ExecuteNonQuery();

                // If successful, return a success response
                return Ok("User created successfully!");
            }
            catch (Exception ex)
            {
                // If an error occurs, return an error response
                return BadRequest($"Failed to create user. Error: {ex.Message}");
            }
            finally
            {
                // Close the database connection in the finally block
                _SQLconnection._connection.Close();
            }
        }
    }
}

