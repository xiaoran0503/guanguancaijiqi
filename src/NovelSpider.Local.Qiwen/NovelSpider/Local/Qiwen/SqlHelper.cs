using System;
using System.Collections;
using System.Data;
using Microsoft.Data.SqlClient;
using NovelSpider.Config;

namespace NovelSpider.Local.Qiwen;

public abstract class SqlHelper
{
	public static string ConnectionString;

	private static Hashtable hashtable_0;

	static SqlHelper()
	{
		ConnectionString = Configs.BaseConfig.ConnectionString;
		hashtable_0 = Hashtable.Synchronized(new Hashtable());
	}

	public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
	{
		hashtable_0[cacheKey] = commandParameters;
	}

	public static DataTable ExecuteDataTable(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
	{
		SqlCommand sqlCommand = new SqlCommand();
		SqlConnection sqlConnection = new SqlConnection(connectionString);
		DataTable dataTable = new DataTable();
		try
		{
			LanmPwuuE(sqlCommand, sqlConnection, null, cmdType, cmdText, commandParameters);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);
			sqlCommand.Parameters.Clear();
			return dataTable;
		}
		catch
		{
			sqlConnection.Close();
			throw;
		}
	}

	public static int ExecuteNonQuery(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
	{
		SqlCommand sqlCommand = new SqlCommand();
		LanmPwuuE(sqlCommand, conn, null, cmdType, cmdText, commandParameters);
		int result = sqlCommand.ExecuteNonQuery();
		sqlCommand.Parameters.Clear();
		return result;
	}

	public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
	{
		SqlCommand sqlCommand = new SqlCommand();
		LanmPwuuE(sqlCommand, trans.Connection, trans, cmdType, cmdText, commandParameters);
		int result = sqlCommand.ExecuteNonQuery();
		sqlCommand.Parameters.Clear();
		return result;
	}

	public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
	{
		SqlCommand sqlCommand = new SqlCommand();
		using SqlConnection sqlConnection_ = new SqlConnection(connectionString);
		LanmPwuuE(sqlCommand, sqlConnection_, null, cmdType, cmdText, commandParameters);
		int result = sqlCommand.ExecuteNonQuery();
		sqlCommand.Parameters.Clear();
		return result;
	}

	public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
	{
		SqlCommand sqlCommand = new SqlCommand();
		SqlConnection sqlConnection = new SqlConnection(connectionString);
		try
		{
			LanmPwuuE(sqlCommand, sqlConnection, null, cmdType, cmdText, commandParameters);
			SqlDataReader result = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
			sqlCommand.Parameters.Clear();
			return result;
		}
		catch
		{
			sqlConnection.Close();
			throw;
		}
	}

	public static object ExecuteScalar(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
	{
		SqlCommand sqlCommand = new SqlCommand();
		LanmPwuuE(sqlCommand, conn, null, cmdType, cmdText, commandParameters);
		object result = sqlCommand.ExecuteScalar();
		sqlCommand.Parameters.Clear();
		return result;
	}

	public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
	{
		SqlCommand sqlCommand = new SqlCommand();
		using SqlConnection sqlConnection_ = new SqlConnection(connectionString);
		LanmPwuuE(sqlCommand, sqlConnection_, null, cmdType, cmdText, commandParameters);
		object result = sqlCommand.ExecuteScalar();
		sqlCommand.Parameters.Clear();
		return result;
	}

	public static SqlParameter[] GetCachedParameters(string cacheKey)
	{
		SqlParameter[] array = (SqlParameter[])hashtable_0[cacheKey];
		if (array == null)
		{
			return null;
		}
		SqlParameter[] array2 = new SqlParameter[array.Length];
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			array2[i] = (SqlParameter)((ICloneable)array[i]).Clone();
		}
		return array2;
	}

	private static void LanmPwuuE(SqlCommand sqlCommand_0, SqlConnection sqlConnection_0, SqlTransaction sqlTransaction_0, CommandType commandType_0, string string_0, SqlParameter[] sqlParameter_0)
	{
		if (sqlConnection_0.State != ConnectionState.Open)
		{
			sqlConnection_0.Open();
		}
		sqlCommand_0.Connection = sqlConnection_0;
		sqlCommand_0.CommandText = string_0;
		if (sqlTransaction_0 != null)
		{
			sqlCommand_0.Transaction = sqlTransaction_0;
		}
		sqlCommand_0.CommandType = commandType_0;
		if (sqlParameter_0 != null)
		{
			foreach (SqlParameter value in sqlParameter_0)
			{
				sqlCommand_0.Parameters.Add(value);
			}
		}
	}
}
