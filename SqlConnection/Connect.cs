﻿using MySqlConnector;

namespace BackendAPI.SqlConnection
{
    // Connect to sql and then use it on other classes
	public class Connect
	{
        public MySqlConnection _connection;
        // Hard code key is not essential, but to save time
        public readonly string _connectionString = "removed";

        public Connect()
        {
            _connection = new MySqlConnection(_connectionString);
        }

        // Open the MySQL connection if it's not open
        public void EnsureConnectionOpen()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        // Close the MySQL connection if it's open
        public void EnsureConnectionClosed()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}

