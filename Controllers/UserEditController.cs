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
    public class UserEditController : ControllerBase
    {
        private readonly Connect _SQLconnection;

        // get connected sql from extension class
        public UserEditController()
        {
            _SQLconnection = new Connect();
        }

        [Route("/users/{username}")]
        [HttpPut]
        public IActionResult UpdateUser(string username, [FromBody] IndividualModel individualModel)
        {
            // string username will take value of the end point {username}
            try
            {
                _SQLconnection.EnsureConnectionOpen();

                // Create a SQL command to update the user's bio based on username
                using var cmd = new MySqlCommand(
                    "UPDATE users SET bio = @bio WHERE username = @username",
                    _SQLconnection._connection
                );

                // Add the parameters
                cmd.Parameters.AddWithValue("@username", individualModel.username);
                cmd.Parameters.AddWithValue("@bio", individualModel.bio);

                // Execute the SQL command to update the user's bio
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // User updated successfully
                    return Ok("User's bio updated successfully");
                }
                else
                {
                    // No rows were affected, indicating the user with the given username doesn't exist
                    return NotFound("User not found");
                }
            }
            catch (Exception ex)
            {
                // If an error occurs, return an error response
                return BadRequest($"Failed to update user's bio. Error: {ex.Message}");
            }
            finally
            {
                _SQLconnection.EnsureConnectionClosed();
            }
        }


        [Route("/users/{username}")]
        [HttpDelete]
        public IActionResult DeleteUser(string username)
        {
            try
            {
                _SQLconnection.EnsureConnectionOpen();

                // Create a SQL command to delete the user with the given username
                using var cmd = new MySqlCommand(
                    "DELETE FROM users WHERE username = @username",
                    _SQLconnection._connection
                );

                // Add the username parameter
                cmd.Parameters.AddWithValue("@username", username);

                // Execute the SQL command to delete the user
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // User deleted successfully
                    return Ok("User deleted successfully");
                }
                else
                {
                    // No rows were affected, indicating the user with the given username doesn't exist
                    return NotFound("User not found");
                }
            }
            catch (Exception ex)
            {
                // If an error occurs, return an error response
                return BadRequest($"Failed to delete user. Error: {ex.Message}");
            }
            finally
            {
                _SQLconnection.EnsureConnectionClosed();
            }
        }



    }
}
