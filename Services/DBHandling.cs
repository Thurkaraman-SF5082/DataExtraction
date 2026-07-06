using System.Data;
using DataExtraction.Interfaces;
using Npgsql;

namespace DataExtraction.Services
{
    public class DBHandling
    {
        public static async Task<int> ExecuteTransactionAsync(string connectionStringPsql, string query, object? parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                await using NpgsqlConnection connection = new(connectionStringPsql);
                if (connection.State == ConnectionState.Closed)
                {
                    await connection.OpenAsync();
                }
                await using NpgsqlTransaction transaction = await connection.BeginTransactionAsync();
                try
                {
                    await using NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        switch (parameters)
                        {
                            case Dictionary<string, object?> dictParams:
                                AddParameters(command, dictParams);
                                break;
                            case NpgsqlParameter[] parameterArray:
                                command.Parameters.AddRange(parameterArray);
                                break;
                            default:
                                AddParameters(command, parameters);
                                break;
                        }
                    }
                    int result = await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    // LogHandler.Instance.WriteLog($"Error ExecuteTransactionAsync StaticHolder: {ex.Message}");
                    Console.WriteLine(ex.Message);
                    await transaction.RollbackAsync();
                    return -1;
                }
            }
            catch (Exception ex)
            {
                // LogHandler.Instance.WriteLog($"Error during connection ExecuteTransactionAsync: {ex.Message}");
                // Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException ?? ex);
                return -1;
            }
        }
        public static async Task<T?> ExecuteScalarAsync<T>(string connectionString, string query, object? parameters = null)
        {
            try
            {
                await using NpgsqlConnection connection = new(connectionString);
                if (connection.State == ConnectionState.Closed)
                {
                    await connection.OpenAsync();
                }
                await using NpgsqlCommand command = new NpgsqlCommand(query, connection);
                if (parameters != null)
                {
                    switch (parameters)
                    {
                        case Dictionary<string, object?> dictParams:
                            AddParameters(command, dictParams);
                            break;
                        case NpgsqlParameter[] npgsqlParameters:
                            command.Parameters.AddRange(npgsqlParameters);
                            break;
                        default:
                            AddParameters(command, parameters);
                            break;
                    }
                }
                var result = await command.ExecuteScalarAsync();
                if (result == null || result == DBNull.Value)
                {
                    return default(T);
                }
                return (T)Convert.ChangeType(result, typeof(T));
            }
            catch (Exception ex)
            {
                // LogHandler.Instance.WriteLog($"Error ExecuteScalarAsync DbModel: {ex.Message}");
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }
        public static async Task<DataTable?> ExecuteQueryAsync(string connectionStringPsql, string query, object? parameters = null)
        {
            try
            {
                await using NpgsqlConnection connection = new(connectionStringPsql);
                if (connection.State == ConnectionState.Closed)
                {
                    await connection.OpenAsync();
                }
                await using NpgsqlCommand command = new NpgsqlCommand(query, connection);
                if (parameters != null)
                {
                    switch (parameters)
                    {
                        case Dictionary<string, object?> dictParams:
                            AddParameters(command, dictParams);
                            break;
                        case NpgsqlParameter[] parameterArray:
                            command.Parameters.AddRange(parameterArray);
                            break;
                        default:
                            AddParameters(command, parameters);
                            break;
                    }
                }
                DataTable dataTable = new DataTable();
                using NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                // LogHandler.Instance.WriteLog(
                //     LogHandler.Instance.IsDetailedLoggingEnabled()
                //         ? $"Error ExecuteQueryAsync StaticHolder: {ex.Message} {ex.InnerException?.Message}"
                //         : $"Error ExecuteQueryAsync StaticHolder: {ex.InnerException?.Message}");
                Console.WriteLine($"{ex.Message} {ex.InnerException?.Message}");
                Console.WriteLine($"{ex.InnerException?.Message}");
                return null;
            }
        }
        public static async IAsyncEnumerable<T> FetchDataFromDatabaseAsync<T>(string connectionString, string query, Func<NpgsqlDataReader, T> mapFunction, params NpgsqlParameter[] parameters)
        {
            await using NpgsqlConnection connection = new(connectionString);
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync();
            }
            await using NpgsqlCommand command = new NpgsqlCommand(query, connection);
            command.Parameters.AddRange(parameters);
            await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                yield return mapFunction(reader);
            }
        }
        private static void AddParameters(NpgsqlCommand command, Dictionary<string, object?> parameters)
        {
            foreach (var keyValuePair in parameters)
            {
                var value = keyValuePair.Value;
                if (value is Array array)
                {
                    if (array.GetType().GetElementType() == typeof(string))
                    {
                        command.Parameters.AddWithValue($"@{keyValuePair.Key}", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Text, array);
                    }
                    else if (array.GetType().GetElementType() == typeof(long))
                    {
                        command.Parameters.AddWithValue($"@{keyValuePair.Key}", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Bigint, array);
                    }
                    else if (array.GetType().GetElementType() == typeof(int))
                    {
                        command.Parameters.AddWithValue($"@{keyValuePair.Key}", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Integer, array);
                    }
                    else if (array.GetType().GetElementType() == typeof(object))
                    {
                        var elementType = array.Cast<object>().FirstOrDefault()?.GetType();
                        if (elementType == typeof(string))
                        {
                            command.Parameters.AddWithValue($"@{keyValuePair.Key}", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Text, array);
                        }
                        else if (elementType == typeof(long))
                        {
                            command.Parameters.AddWithValue($"@{keyValuePair.Key}", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Bigint, array);
                        }
                        else if (elementType == typeof(int))
                        {
                            command.Parameters.AddWithValue($"@{keyValuePair.Key}", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Integer, array);
                        }
                        else
                        {
                            throw new InvalidOperationException($"Unsupported array element type: {elementType}");
                        }
                    }
                }
                else if (value is string strValue)
                {
                    command.Parameters.AddWithValue($"@{keyValuePair.Key}", NpgsqlTypes.NpgsqlDbType.Text, strValue);
                }
                else if (value is DateTime dateTimeValue)
                {
                    command.Parameters.AddWithValue($"@{keyValuePair.Key}", NpgsqlTypes.NpgsqlDbType.Timestamp, dateTimeValue);
                }
                else if (value is long)
                {
                    command.Parameters.AddWithValue($"@{keyValuePair.Key}", NpgsqlTypes.NpgsqlDbType.Bigint, value);
                }
                else if (value is int)
                {
                    command.Parameters.AddWithValue($"@{keyValuePair.Key}", NpgsqlTypes.NpgsqlDbType.Integer, value);
                }
                else if (value is bool)
                {
                    command.Parameters.AddWithValue($"@{keyValuePair.Key}", NpgsqlTypes.NpgsqlDbType.Boolean, value);
                }
                else
                    command.Parameters.AddWithValue($"@{keyValuePair.Key}", value ?? DBNull.Value);
            }
        }
        private static void AddParameters(NpgsqlCommand command, object? parameters)
        {
            if (parameters != null)
            {
                var properties = parameters.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var value = property.GetValue(parameters);
                    if (value is Array array)
                    {
                        if (array.GetType().GetElementType() == typeof(string))
                        {
                            command.Parameters.AddWithValue($"@{property.Name}", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Text, array);
                        }
                        else if (array.GetType().GetElementType() == typeof(long))
                        {
                            command.Parameters.AddWithValue($"@{property.Name}", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Bigint, array);
                        }
                        else if (array.GetType().GetElementType() == typeof(int))
                        {
                            command.Parameters.AddWithValue($"@{property.Name}", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Integer, array);
                        }
                        else if (array.GetType().GetElementType() == typeof(object))
                        {
                            var elementType = array.Cast<object>().FirstOrDefault()?.GetType();
                            if (elementType == typeof(string))
                            {
                                command.Parameters.AddWithValue($"@{property.Name}", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Text, array);
                            }
                            else if (elementType == typeof(long))
                            {
                                command.Parameters.AddWithValue($"@{property.Name}", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Bigint, array);
                            }
                            else if (elementType == typeof(int))
                            {
                                command.Parameters.AddWithValue($"@{property.Name}", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Integer, array);
                            }
                            else
                            {
                                throw new InvalidOperationException($"Unsupported array element type: {elementType}");
                            }
                        }
                    }
                    else
                        command.Parameters.AddWithValue($"@{property.Name}", value ?? DBNull.Value);
                }
            }
        }



    }
}