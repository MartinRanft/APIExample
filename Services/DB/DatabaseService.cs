using System.Data;

using ByteWizardApi.Interfaces.DB;

using MySqlConnector;

namespace ByteWizardApi.Services.DB
{
    /// <summary>
    /// Provides database interaction functionalities, including execution of stored procedures,
    /// SQL queries, and database functions with MySQL as the backend.
    /// </summary>
    public sealed class DatabaseService : IDatabaseService
    {
        
        /// <summary>
        /// Stores the connection string used for establishing a connection to the MySQL database.
        /// Depending on the build configuration, it selects either the development or production
        /// connection string from the application configuration.
        /// </summary>
        private readonly string _connectionString;
        
        /// <summary>
        /// Provides database interaction functionalities, including execution of stored procedures,
        /// SQL queries, and database functions with MySQL as the backend.
        /// </summary>
        public DatabaseService(IConfiguration configuration)
        {
            if(configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration), "Configuration is required.");
            }
#if DEBUG
            _connectionString = configuration.GetConnectionString("MariaDBConnectionDev") ?? throw new ArgumentNullException("", "Connection string is not configured.");
#else
            _connectionString = configuration.GetConnectionString("MariaDBConnection") ?? throw new ArgumentNullException("", "Connection string is not configured.");
#endif
        }


        /// <summary>
        /// Executes a stored procedure on the MySQL database using the provided procedure name
        /// and parameters, and returns the result as a DataTable.
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure to be executed.</param>
        /// <param name="parameters">An array of MySqlParameter instances to be used as parameters for the stored procedure.</param>
        /// <returns>A DataTable containing the result set produced by the execution of the stored procedure.</returns>
        public DataTable ExecuteStoredProcedure(string procedureName, MySqlParameter[] parameters)
        {
            using MySqlConnection connection = new(_connectionString);
            connection.Open();

            using MySqlCommand command = new(procedureName, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            using MySqlDataAdapter adapter = new(command);
            DataTable result = new();
            adapter.Fill(result);
            
            // Check for and handle DBNull values in the result set.
            for(int rowIndex = 0; rowIndex < result.Rows.Count; rowIndex++)
            {
                for(int colIndex = 0; colIndex < result.Columns.Count; colIndex++)
                {
                    if(result.Rows[rowIndex][colIndex] == DBNull.Value && result.Columns[colIndex].DataType == typeof(DateTime))
                    {
                        // Set null for DateTime columns that contain DBNull values.
                        result.Rows[rowIndex][colIndex] = DBNull.Value;
                    }
                    else if(result.Rows[rowIndex][colIndex] == DBNull.Value)
                    {
                        // other Column Types can use empty strings or default values here
                        result.Rows[rowIndex][colIndex] = string.Empty;
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Executes an SQL query against the MySQL database and returns the result as a DataTable.
        /// </summary>
        /// <param name="query">The SQL query string to be executed.</param>
        /// <param name="parameters">An array of MySqlParameter objects representing the parameters to be passed to the SQL query.</param>
        /// <returns>A DataTable containing the result set of the executed SQL query.</returns>
        public DataTable ExecuteSqlQuery(string query, MySqlParameter[] parameters)
        {
            using MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();

            using MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddRange(parameters);

            using MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable result = new();
            adapter.Fill(result);

            return result;
        }


        /// <summary>
        /// Executes a MySQL database function using the provided function name and parameters,
        /// returning the result set in a DataTable.
        /// </summary>
        /// <param name="functionName">The name of the database function to execute.</param>
        /// <param name="parameters">An array of MySqlParameters to be passed to the function.</param>
        /// <returns>A DataTable containing the results of the executed function.</returns>
        public DataTable ExecuteFunction(string functionName, MySqlParameter[] parameters)
        {
            using MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();

            using MySqlCommand command = new MySqlCommand(functionName, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            using MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable result = new DataTable();
            adapter.Fill(result);

            return result;
        }


        /// <summary>
        /// Executes a scalar database function using the specified function name and parameters,
        /// and retrieves a single value result. If the result is null or DBNull, a default value is returned.
        /// </summary>
        /// <param name="functionName">The name of the database function to execute.</param>
        /// <param name="parameters">An array of MySqlParameter objects to pass to the function.</param>
        /// <returns>The single result of the scalar function execution, or a default value if the result is null.</returns>
        public object ExecuteScalarFunction(string functionName, MySqlParameter[] parameters)
        {
            using MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();

            using MySqlCommand command = new MySqlCommand(functionName, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            command.ExecuteScalar();

            object? result = command.Parameters[parameters.Length - 1].Value;
            
            if(result == null || result == DBNull.Value)
            {
                return 0;
            }

            return result;
        }
    }
}