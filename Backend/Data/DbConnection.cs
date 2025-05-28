using MySql.Data.MySqlClient;

namespace Backend.Data
{
    public class DbConnection
    {
        private readonly string _connectionString;

        public DbConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
} 