using System.Data;
using Microsoft.Data.Sqlite;

namespace NovelSpider.Common;

/// <summary>
/// SQLite 访问帮助类。已从已停更的 System.Data.SQLite.Core 迁移至官方维护的 Microsoft.Data.Sqlite。
/// 公共 API（ExecuteDataset / ExecuteNonQuery / ExecuteScalar / ExecuteReader / ExecuteDataRow）保持兼容，
/// DataSet 由 SqliteDataReader 手动填充（Microsoft.Data.Sqlite 无 DataAdapter）。
/// </summary>
public sealed class SQLiteHelper
{
	private SQLiteHelper()
	{
	}

	public static DataRow ExecuteDataRow(string string_0, string string_1, params IDataParameter[] idataParameter_0)
	{
		DataSet dataSet = ExecuteDataset(string_0, string_1, idataParameter_0);
		if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
		{
			return null;
		}
		return dataSet.Tables[0].Rows[0];
	}

	public static DataSet ExecuteDataset(IDbConnection idbConnection_0, string string_0)
	{
		return ExecuteDataset(idbConnection_0, string_0, (IDataParameter[])null);
	}

	public static DataSet ExecuteDataset(string string_0, string string_1)
	{
		return ExecuteDataset(string_0, string_1, (IDataParameter[])null);
	}

	public static DataSet ExecuteDataset(IDbConnection idbConnection_0, string string_0, params IDataParameter[] idataParameter_0)
	{
		SqliteCommand sqliteCommand = new SqliteCommand
		{
			Connection = (SqliteConnection)idbConnection_0,
			CommandText = string_0,
			CommandType = CommandType.Text
		};
		if (idataParameter_0 != null)
		{
			foreach (IDataParameter value in idataParameter_0)
			{
				sqliteCommand.Parameters.Add(value);
			}
		}
		DataSet dataSet = FillDataSet(sqliteCommand);
		sqliteCommand.Parameters.Clear();
		return dataSet;
	}

	public static DataSet ExecuteDataset(string string_0, string string_1, params IDataParameter[] idataParameter_0)
	{
		using SqliteConnection sqliteConnection = new SqliteConnection(string_0);
		sqliteConnection.Open();
		return ExecuteDataset(sqliteConnection, string_1, idataParameter_0);
	}

	public static int ExecuteNonQuery(IDbConnection idbConnection_0, string string_0, params IDataParameter[] idataParameter_0)
	{
		SqliteCommand sqliteCommand = new SqliteCommand
		{
			Connection = (SqliteConnection)idbConnection_0,
			CommandText = string_0,
			CommandType = CommandType.Text
		};
		if (idataParameter_0 != null)
		{
			foreach (IDataParameter value in idataParameter_0)
			{
				sqliteCommand.Parameters.Add(value);
			}
		}
		int result = sqliteCommand.ExecuteNonQuery();
		sqliteCommand.Parameters.Clear();
		return result;
	}

	public static int ExecuteNonQuery(string string_0, string string_1, params IDataParameter[] idataParameter_0)
	{
		using SqliteConnection sqliteConnection = new SqliteConnection(string_0);
		sqliteConnection.Open();
		return ExecuteNonQuery(sqliteConnection, string_1, idataParameter_0);
	}

	public static IDataReader ExecuteReader(string string_0, string string_1)
	{
		return ExecuteReader(string_0, string_1, (IDataParameter[])null);
	}

	public static IDataReader ExecuteReader(string string_0, string string_1, params IDataParameter[] idataParameter_0)
	{
		SqliteConnection sqliteConnection = new SqliteConnection(string_0);
		sqliteConnection.Open();
		try
		{
			return smethod_0(sqliteConnection, null, string_1, idataParameter_0, bool_0: false);
		}
		catch
		{
			sqliteConnection.Close();
			throw;
		}
	}

	public static object ExecuteScalar(IDbConnection idbConnection_0, string string_0)
	{
		return ExecuteScalar(idbConnection_0, string_0, (IDataParameter[])null);
	}

	public static object ExecuteScalar(string string_0, string string_1)
	{
		return ExecuteScalar(string_0, string_1, (IDataParameter[])null);
	}

	public static object ExecuteScalar(IDbConnection idbConnection_0, string string_0, params IDataParameter[] idataParameter_0)
	{
		SqliteCommand sqliteCommand = new SqliteCommand
		{
			Connection = (SqliteConnection)idbConnection_0,
			CommandText = string_0,
			CommandType = CommandType.Text
		};
		if (idataParameter_0 != null)
		{
			foreach (IDataParameter value in idataParameter_0)
			{
				sqliteCommand.Parameters.Add(value);
			}
		}
		object result = sqliteCommand.ExecuteScalar();
		sqliteCommand.Parameters.Clear();
		return result;
	}

	public static object ExecuteScalar(string string_0, string string_1, params IDataParameter[] idataParameter_0)
	{
		using SqliteConnection sqliteConnection = new SqliteConnection(string_0);
		sqliteConnection.Open();
		return ExecuteScalar(sqliteConnection, string_1, idataParameter_0);
	}

	private static IDataReader smethod_0(IDbConnection idbConnection_0, SqliteTransaction sqliteTransaction_0, string string_0, IDataParameter[] idataParameter_0, bool bool_0)
	{
		SqliteCommand sqliteCommand = new SqliteCommand
		{
			Connection = (SqliteConnection)idbConnection_0,
			Transaction = sqliteTransaction_0,
			CommandText = string_0,
			CommandType = CommandType.Text
		};
		if (idataParameter_0 != null)
		{
			foreach (IDataParameter value in idataParameter_0)
			{
				sqliteCommand.Parameters.Add(value);
			}
		}
		IDataReader result = bool_0 ? sqliteCommand.ExecuteReader() : sqliteCommand.ExecuteReader(CommandBehavior.CloseConnection);
		sqliteCommand.Parameters.Clear();
		return result;
	}

	private static DataSet FillDataSet(SqliteCommand sqliteCommand)
	{
		DataSet dataSet = new DataSet();
		using (SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
		{
			DataTable dataTable = new DataTable();
			for (int i = 0; i < sqliteDataReader.FieldCount; i++)
			{
				dataTable.Columns.Add(sqliteDataReader.GetName(i), typeof(object));
			}
			object[] values = new object[sqliteDataReader.FieldCount];
			while (sqliteDataReader.Read())
			{
				sqliteDataReader.GetValues(values);
				dataTable.Rows.Add(values);
			}
			dataSet.Tables.Add(dataTable);
		}
		return dataSet;
	}
}
