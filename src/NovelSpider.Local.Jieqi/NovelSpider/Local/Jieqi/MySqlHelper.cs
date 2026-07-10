using System;
using System.Collections;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using NovelSpider.Common;
using MySqlConnector;
using NovelSpider.Config;

namespace NovelSpider.Local.Jieqi;

public abstract class MySqlHelper
{
	public static string ConnectionString;

	private static Hashtable hashtable_0;

	static MySqlHelper()
	{
		ConnectionString = NormalizeConnectionString(Configs.BaseConfig.ConnectionString);
		hashtable_0 = new Hashtable();
	}

	private static void smethod_00()
	{
		smethod_11();
	}

	private static void smethod_11()
	{
		ConnectionString = NormalizeConnectionString(Config.ConnectionString);
		hashtable_0 = Hashtable.Synchronized(new Hashtable());
	}

	public static void CacheParameters(string string_0, params MySqlParameter[] mySqlParameter_0)
	{
		hashtable_0[string_0] = mySqlParameter_0;
	}

	public static DataTable ExecuteDataTable(string string_0, CommandType commandType_0, string string_1, params MySqlParameter[] mySqlParameter_0)
	{
		using IDisposable scope = PerformanceTelemetry.Measure("mysql", "datatable", GetCommandSubject(string_1));
		MySqlCommand mySqlCommand = new MySqlCommand();
		MySqlConnection mySqlConnection = new MySqlConnection(NormalizeConnectionString(string_0));
		DataTable dataTable = new DataTable();
		try
		{
			smethod_0(mySqlCommand, mySqlConnection, null, commandType_0, string_1, mySqlParameter_0);
			new MySqlDataAdapter(mySqlCommand).Fill(dataTable);
			mySqlCommand.Parameters.Clear();
			mySqlConnection.Close();
			return dataTable;
		}
		catch
		{
			mySqlConnection.Close();
			throw;
		}
	}

	public static DataTable ExecuteDataTable(MySqlTransaction mySqlTransaction_0, CommandType commandType_0, string string_0, params MySqlParameter[] mySqlParameter_0)
	{
		using IDisposable scope = PerformanceTelemetry.Measure("mysql", "datatable_tx", GetCommandSubject(string_0));
		MySqlCommand mySqlCommand = new MySqlCommand();
		DataTable dataTable = new DataTable();
		smethod_0(mySqlCommand, mySqlTransaction_0.Connection, mySqlTransaction_0, commandType_0, string_0, mySqlParameter_0);
		new MySqlDataAdapter(mySqlCommand).Fill(dataTable);
		mySqlCommand.Parameters.Clear();
		return dataTable;
	}

	public static int ExecuteNonQuery(MySqlConnection mySqlConnection_0, CommandType commandType_0, string string_0, params MySqlParameter[] mySqlParameter_0)
	{
		using IDisposable scope = PerformanceTelemetry.Measure("mysql", "nonquery_conn", GetCommandSubject(string_0));
		MySqlCommand mySqlCommand = new MySqlCommand();
		smethod_0(mySqlCommand, mySqlConnection_0, null, commandType_0, string_0, mySqlParameter_0);
		int result = mySqlCommand.ExecuteNonQuery();
		mySqlCommand.Parameters.Clear();
		return result;
	}

	public static int ExecuteNonQuery(MySqlTransaction mySqlTransaction_0, CommandType commandType_0, string string_0, params MySqlParameter[] mySqlParameter_0)
	{
		using IDisposable scope = PerformanceTelemetry.Measure("mysql", "nonquery_tx", GetCommandSubject(string_0));
		MySqlCommand mySqlCommand = new MySqlCommand();
		smethod_0(mySqlCommand, mySqlTransaction_0.Connection, mySqlTransaction_0, commandType_0, string_0, mySqlParameter_0);
		int result = mySqlCommand.ExecuteNonQuery();
		mySqlCommand.Parameters.Clear();
		return result;
	}

	public static long ExecuteNonQueryWithLastInsertId(MySqlTransaction mySqlTransaction_0, CommandType commandType_0, string string_0, params MySqlParameter[] mySqlParameter_0)
	{
		using IDisposable scope = PerformanceTelemetry.Measure("mysql", "insert_tx_last_id", GetCommandSubject(string_0));
		MySqlCommand mySqlCommand = new MySqlCommand();
		smethod_0(mySqlCommand, mySqlTransaction_0.Connection, mySqlTransaction_0, commandType_0, string_0, mySqlParameter_0);
		mySqlCommand.ExecuteNonQuery();
		long result = mySqlCommand.LastInsertedId;
		mySqlCommand.Parameters.Clear();
		return result;
	}

	public static async Task<int> ExecuteNonQueryAsync(MySqlTransaction mySqlTransaction_0, CommandType commandType_0, string string_0, CancellationToken cancellationToken = default, params MySqlParameter[] mySqlParameter_0)
	{
		using IDisposable scope = PerformanceTelemetry.Measure("mysql", "nonquery_tx_async", GetCommandSubject(string_0));
		MySqlCommand mySqlCommand = new MySqlCommand();
		smethod_0(mySqlCommand, mySqlTransaction_0.Connection, mySqlTransaction_0, commandType_0, string_0, mySqlParameter_0);
		int result = await mySqlCommand.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
		mySqlCommand.Parameters.Clear();
		return result;
	}

	public static async Task<long> ExecuteNonQueryWithLastInsertIdAsync(MySqlTransaction mySqlTransaction_0, CommandType commandType_0, string string_0, CancellationToken cancellationToken = default, params MySqlParameter[] mySqlParameter_0)
	{
		using IDisposable scope = PerformanceTelemetry.Measure("mysql", "insert_tx_last_id_async", GetCommandSubject(string_0));
		MySqlCommand mySqlCommand = new MySqlCommand();
		smethod_0(mySqlCommand, mySqlTransaction_0.Connection, mySqlTransaction_0, commandType_0, string_0, mySqlParameter_0);
		await mySqlCommand.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
		long result = mySqlCommand.LastInsertedId;
		mySqlCommand.Parameters.Clear();
		return result;
	}
	public static int ExecuteNonQuery(string string_0, CommandType commandType_0, string string_1, params MySqlParameter[] mySqlParameter_0)
	{
		using IDisposable scope = PerformanceTelemetry.Measure("mysql", "nonquery", GetCommandSubject(string_1));
		MySqlCommand mySqlCommand = new MySqlCommand();
		using MySqlConnection mySqlConnection_ = new MySqlConnection(NormalizeConnectionString(string_0));
		smethod_0(mySqlCommand, mySqlConnection_, null, commandType_0, string_1, mySqlParameter_0);
		int result = mySqlCommand.ExecuteNonQuery();
		mySqlCommand.Parameters.Clear();
		return result;
	}

	public static void ExecuteInTransaction(Action<MySqlTransaction> action)
	{
		using IDisposable scope = PerformanceTelemetry.Measure("mysql", "transaction");
		using MySqlConnection mySqlConnection = new MySqlConnection(NormalizeConnectionString(ConnectionString));
		mySqlConnection.Open();
		using MySqlTransaction mySqlTransaction = mySqlConnection.BeginTransaction();
		try
		{
			action(mySqlTransaction);
			mySqlTransaction.Commit();
		}
		catch
		{
			try
			{
				mySqlTransaction.Rollback();
			}
			catch
			{
			}
			throw;
		}
	}

	public static T ExecuteInTransaction<T>(Func<MySqlTransaction, T> action)
	{
		using IDisposable scope = PerformanceTelemetry.Measure("mysql", "transaction");
		using MySqlConnection mySqlConnection = new MySqlConnection(NormalizeConnectionString(ConnectionString));
		mySqlConnection.Open();
		using MySqlTransaction mySqlTransaction = mySqlConnection.BeginTransaction();
		try
		{
			T result = action(mySqlTransaction);
			mySqlTransaction.Commit();
			return result;
		}
		catch
		{
			try
			{
				mySqlTransaction.Rollback();
			}
			catch
			{
			}
			throw;
		}
	}

	public static async Task ExecuteInTransactionAsync(Func<MySqlTransaction, CancellationToken, Task> action, CancellationToken cancellationToken = default)
	{
		using IDisposable scope = PerformanceTelemetry.Measure("mysql", "transaction_async");
		await using MySqlConnection mySqlConnection = new MySqlConnection(NormalizeConnectionString(ConnectionString));
		await mySqlConnection.OpenAsync(cancellationToken).ConfigureAwait(false);
		await using MySqlTransaction mySqlTransaction = await mySqlConnection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
		try
		{
			await action(mySqlTransaction, cancellationToken).ConfigureAwait(false);
			await mySqlTransaction.CommitAsync(cancellationToken).ConfigureAwait(false);
		}
		catch
		{
			try
			{
				await mySqlTransaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
			}
			catch
			{
			}
			throw;
		}
	}

	public static async Task<T> ExecuteInTransactionAsync<T>(Func<MySqlTransaction, CancellationToken, Task<T>> action, CancellationToken cancellationToken = default)
	{
		using IDisposable scope = PerformanceTelemetry.Measure("mysql", "transaction_async");
		await using MySqlConnection mySqlConnection = new MySqlConnection(NormalizeConnectionString(ConnectionString));
		await mySqlConnection.OpenAsync(cancellationToken).ConfigureAwait(false);
		await using MySqlTransaction mySqlTransaction = await mySqlConnection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
		try
		{
			T result = await action(mySqlTransaction, cancellationToken).ConfigureAwait(false);
			await mySqlTransaction.CommitAsync(cancellationToken).ConfigureAwait(false);
			return result;
		}
		catch
		{
			try
			{
				await mySqlTransaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
			}
			catch
			{
			}
			throw;
		}
	}
	public static MySqlDataReader ExecuteReader(string string_0, CommandType commandType_0, string string_1, params MySqlParameter[] mySqlParameter_0)
	{
		using IDisposable scope = PerformanceTelemetry.Measure("mysql", "reader", GetCommandSubject(string_1));
		MySqlCommand mySqlCommand = new MySqlCommand();
		MySqlConnection mySqlConnection = new MySqlConnection(NormalizeConnectionString(string_0));
		try
		{
			smethod_0(mySqlCommand, mySqlConnection, null, commandType_0, string_1, mySqlParameter_0);
			MySqlDataReader result = mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
			mySqlCommand.Parameters.Clear();
			return result;
		}
		catch
		{
			mySqlConnection.Close();
			throw;
		}
	}

	public static object ExecuteScalar(MySqlConnection mySqlConnection_0, CommandType commandType_0, string string_0, params MySqlParameter[] mySqlParameter_0)
	{
		using IDisposable scope = PerformanceTelemetry.Measure("mysql", "scalar_conn", GetCommandSubject(string_0));
		MySqlCommand mySqlCommand = new MySqlCommand();
		smethod_0(mySqlCommand, mySqlConnection_0, null, commandType_0, string_0, mySqlParameter_0);
		object result = mySqlCommand.ExecuteScalar();
		mySqlCommand.Parameters.Clear();
		return result;
	}

	public static object ExecuteScalar(MySqlTransaction mySqlTransaction_0, CommandType commandType_0, string string_0, params MySqlParameter[] mySqlParameter_0)
	{
		using IDisposable scope = PerformanceTelemetry.Measure("mysql", "scalar_tx", GetCommandSubject(string_0));
		MySqlCommand mySqlCommand = new MySqlCommand();
		smethod_0(mySqlCommand, mySqlTransaction_0.Connection, mySqlTransaction_0, commandType_0, string_0, mySqlParameter_0);
		object result = mySqlCommand.ExecuteScalar();
		mySqlCommand.Parameters.Clear();
		return result;
	}

	public static object ExecuteScalar(string string_0, CommandType commandType_0, string string_1, params MySqlParameter[] mySqlParameter_0)
	{
		using IDisposable scope = PerformanceTelemetry.Measure("mysql", "scalar", GetCommandSubject(string_1));
		MySqlCommand mySqlCommand = new MySqlCommand();
		using MySqlConnection mySqlConnection_ = new MySqlConnection(NormalizeConnectionString(string_0));
		smethod_0(mySqlCommand, mySqlConnection_, null, commandType_0, string_1, mySqlParameter_0);
		object result = mySqlCommand.ExecuteScalar();
		mySqlCommand.Parameters.Clear();
		return result;
	}

	public static MySqlParameter[] GetCachedParameters(string string_0)
	{
		MySqlParameter[] array = (MySqlParameter[])hashtable_0[string_0];
		if (array == null)
		{
			return null;
		}
		MySqlParameter[] array2 = new MySqlParameter[array.Length];
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			array2[i] = (MySqlParameter)((ICloneable)array[i]).Clone();
		}
		return array2;
	}

	private static void smethod_0(MySqlCommand mySqlCommand_0, MySqlConnection mySqlConnection_0, MySqlTransaction mySqlTransaction_0, CommandType commandType_0, string string_0, MySqlParameter[] mySqlParameter_0)
	{
		if (mySqlConnection_0.State != ConnectionState.Open)
		{
			mySqlConnection_0.Open();
		}
		mySqlCommand_0.Connection = mySqlConnection_0;
		mySqlCommand_0.CommandText = string_0;
		if (mySqlTransaction_0 != null)
		{
			mySqlCommand_0.Transaction = mySqlTransaction_0;
		}
		mySqlCommand_0.CommandType = commandType_0;
		if (mySqlParameter_0 != null)
		{
			foreach (MySqlParameter value in mySqlParameter_0)
			{
				mySqlCommand_0.Parameters.Add(value);
			}
		}
	}

	private static string NormalizeConnectionString(string connectionString)
	{
		if (Configs.BaseConfig == null)
		{
			return connectionString;
		}
		return DatabaseCompatibilityProfile.NormalizeConnectionString(connectionString, Configs.BaseConfig.DatabaseServerType, Configs.BaseConfig.DatabaseServerMajorVersion);
	}

	private static string GetCommandSubject(string commandText)
	{
		if (string.IsNullOrWhiteSpace(commandText))
		{
			return string.Empty;
		}
		commandText = commandText.Replace('\r', ' ').Replace('\n', ' ').Trim();
		return commandText.Length <= 120 ? commandText : commandText.Substring(0, 120);
	}
}
