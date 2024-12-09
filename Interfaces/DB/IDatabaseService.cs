using System.Data;

using MySqlConnector;

namespace ByteWizardApi.Interfaces.DB
{
    /// <summary>
    /// Provides methods for executing database stored procedures, functions,
    /// and SQL queries, returning results in various forms such as DataTable or scalar values.
    /// </summary>
    public interface IDatabaseService
    {
        /// <summary>
        /// Executes a stored procedure within the database and returns the results as a DataTable.
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">An array of MySqlParameter objects representing the input parameters for the stored procedure.</param>
        /// <returns>A DataTable containing the results of the executed stored procedure.</returns>
        DataTable ExecuteStoredProcedure(string procedureName, MySqlParameter[] parameters);


        /// <summary>
        /// Executes a MySQL database function using the provided function name and parameters,
        /// returning the result set in a DataTable.
        /// </summary>
        /// <param name="functionName">The name of the database function to execute.</param>
        /// <param name="parameters">An array of MySqlParameters to be passed to the function.</param>
        /// <returns>A DataTable containing the results of the executed function.</returns>
        DataTable ExecuteFunction(string functionName, MySqlParameter[] parameters);


        /// <summary>
        /// Executes a scalar database function using the specified function name and parameters,
        /// and retrieves a single value result. If the result is null or DBNull, a default value is returned.
        /// </summary>
        /// <param name="functionName">The name of the database function to execute.</param>
        /// <param name="parameters">An array of MySqlParameter objects to pass to the function.</param>
        /// <returns>The single result of the scalar function execution, or a default value if the result is null.</returns>
        object ExecuteScalarFunction(string functionName, MySqlParameter[] parameters);


        /// <summary>
        /// Executes a SQL query within the database and returns the results as a DataTable.
        /// </summary>
        /// <param name="query">The SQL query string to execute.</param>
        /// <param name="parameters">An array of MySqlParameter objects representing the input parameters for the SQL query.</param>
        /// <returns>A DataTable containing the results of the executed SQL query.</returns>
        DataTable ExecuteSqlQuery(string query, MySqlParameter[] parameters);
    }
}