using Microsoft.AspNetCore.Mvc;
using System;
using BackendAPI.Model;
using BackendAPI.SqlConnection;
using Microsoft.EntityFrameworkCore; // Import Entity Framework Core
using BackendAPI;
using MySqlConnector;

namespace BackendAPI.Controllers
{
    // Sets the end point route
    [Route("/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly Connect _SQLconnection;

        public LoginController()
        {
            _SQLconnection = new Connect();
        }

        [HttpPost]
        public IActionResult LoginUser([FromBody] LoginModel loginModel)
        {
            try
            {
                _SQLconnection.EnsureConnectionOpen();

                // Create a SQL command to select all columns for the given username and password
                using var cmd = new MySqlCommand(
                    "SELECT * FROM users WHERE username = @username AND password = @password",
                    _SQLconnection._connection
                );

                // Add parameters for username and password
                cmd.Parameters.AddWithValue("@username", loginModel.username);
                cmd.Parameters.AddWithValue("@password", loginModel.password);

                using var reader = cmd.ExecuteReader();

                // Check if a row with the given username and password exists
                if (reader.Read())
                {
                    // Retrieve user data from the reader
                    string username = reader.GetString("username");
                    string password = reader.GetString("password");

                    // Create a User object or DTO and populate it with the retrieved data
                    var user = new UserModel
                    {
                        username = username,
                        password = password,
                    };

                    return Ok("success");
                }
                else
                {
                    // Return a not found response if the username and password combination doesn't exist
                    return NotFound("User not found");
                }
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

