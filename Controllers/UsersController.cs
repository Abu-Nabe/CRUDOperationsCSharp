using Microsoft.AspNetCore.Mvc;
using System;
using BackendAPI.Model;
using BackendAPI.SqlConnection;
using Microsoft.EntityFrameworkCore; // Import Entity Framework Core
using BackendAPI;
using MySqlConnector;


namespace BackendAPI.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Connect _SQLconnection;

        public UserController()
        {
            _SQLconnection = new Connect();
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                _SQLconnection.EnsureConnectionOpen();

                // Create a SQL command to select all users
                using var cmd = new MySqlCommand(
                    "SELECT * FROM users",
                    _SQLconnection._connection
                );

                using var reader = cmd.ExecuteReader();
                var users = new List<UserModel>();

                while (reader.Read())
                {
                    // Retrieve user data from the reader
                    string username = reader.GetString("username");
                    string password = reader.GetString("password");
                    string bio = reader.GetString("bio");

                    // Create a User object or DTO and populate it with the retrieved data
                    var user = new UserModel
                    {
                        username = username,
                        password = password,
                        bio = bio
                    };

                    users.Add(user);
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                // If an error occurs, return an error response
                return BadRequest($"Failed to retrieve users. Error: {ex.Message}");
            }
            finally
            {
                _SQLconnection.EnsureConnectionClosed();
            }
        }

        [Route("/users/{username}")]
        [HttpGet]
        public IActionResult AuthenticateUser(string username)
        {
            try
            {
                _SQLconnection.EnsureConnectionOpen();

                // Create a SQL command to select all columns for the given username
                using var cmd = new MySqlCommand(
                    "SELECT * FROM users WHERE username = @username",
                    _SQLconnection._connection
                );

                // Add the username parameter
                cmd.Parameters.AddWithValue("@username", username);

                using var reader = cmd.ExecuteReader();

                // Check if a row with the given username exists
                if (reader.Read())
                {
                    // Retrieve user data from the reader
                    string fetchedUsername = reader.GetString("username");
                    string bio = reader.GetString("bio");

                    // Create a User object or DTO and populate it with the retrieved data
                    var user = new UserModel
                    {
                        username = fetchedUsername,
                        bio = bio
                    };

                    return Ok(user);
                }
                else
                {
                    // Return a not found response if the username doesn't exist
                    return NotFound("User not found");
                }
            }
            catch (Exception ex)
            {
                // If an error occurs, return an error response
                return BadRequest($"Failed to authenticate user. Error: {ex.Message}");
            }
            finally
            {
                _SQLconnection.EnsureConnectionClosed();
            }
        }


    }
}
