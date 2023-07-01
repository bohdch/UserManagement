using System.Data.SqlClient;

/*namespace UserManagement.Services.Data
{*/
    public class DatabaseService
    {
        // The connection string used to connect to the database.
        private readonly string _connectionString = "Server=localhost;Database = UserManagment;Trusted_Connection=True;";

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
//}
