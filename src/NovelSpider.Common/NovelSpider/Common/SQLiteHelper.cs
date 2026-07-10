using System.Data;
using System.Data.SQLite;

namespace NovelSpider.Common;

public sealed class SQLiteHelper
{
	private SQLiteHelper()
	{
	}

	public static DataRow ExecuteDataRow(string string_0, string string_1, params IDataParameter[] idataParameter_0)
	{
		DataSet dataSet = ExecuteDataset(string_0, string_1, idataParameter_0);
		if (dataSet == null)
		{
			return null;
		}
		if (dataSet.Tables.Count == 0)
		{
			return null;
		}
		if (dataSet.Tables[0].Rows.Count == 0)
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
		SQLiteCommand sQLiteCommand = new SQLiteCommand
		{
			Connection = (SQLiteConnection)idbConnection_0,
			CommandText = string_0,
			CommandType = CommandType.Text
		};
		if (idataParameter_0 != null)
		{
			foreach (IDataParameter value in idataParameter_0)
			{
				sQLiteCommand.Parameters.Add(value);
			}
		}
		SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(sQLiteCommand);
		DataSet dataSet = new DataSet();
		sQLiteDataAdapter.Fill(dataSet);
		if (dataSet.Tables.Count == 0)
		{
			sQLiteDataAdapter.FillSchema(dataSet, SchemaType.Source);
		}
		sQLiteCommand.Parameters.Clear();
		return dataSet;
	}

	public static DataSet ExecuteDataset(string string_0, string string_1, params IDataParameter[] idataParameter_0)
	{
		using SQLiteConnection sQLiteConnection = new SQLiteConnection(string_0);
		sQLiteConnection.Open();
		return ExecuteDataset(sQLiteConnection, string_1, idataParameter_0);
	}

	public static int ExecuteNonQuery(IDbConnection idbConnection_0, string string_0, params IDataParameter[] idataParameter_0)
	{
		SQLiteCommand sQLiteCommand = new SQLiteCommand
		{
			Connection = (SQLiteConnection)idbConnection_0,
			CommandText = string_0,
			CommandType = CommandType.Text
		};
		if (idataParameter_0 != null)
		{
			foreach (IDataParameter value in idataParameter_0)
			{
				sQLiteCommand.Parameters.Add(value);
			}
		}
		int result = sQLiteCommand.ExecuteNonQuery();
		sQLiteCommand.Parameters.Clear();
		return result;
	}

	public static int ExecuteNonQuery(string string_0, string string_1, params IDataParameter[] idataParameter_0)
	{
		using SQLiteConnection sQLiteConnection = new SQLiteConnection(string_0);
		sQLiteConnection.Open();
		return ExecuteNonQuery(sQLiteConnection, string_1, idataParameter_0);
	}

	public static IDataReader ExecuteReader(string string_0, string string_1)
	{
		return ExecuteReader(string_0, string_1, (IDataParameter[])null);
	}

	public static IDataReader ExecuteReader(string string_0, string string_1, params IDataParameter[] idataParameter_0)
	{
		SQLiteConnection sQLiteConnection = new SQLiteConnection(string_0);
		sQLiteConnection.Open();
		try
		{
			return smethod_0(sQLiteConnection, null, string_1, idataParameter_0, bool_0: false);
		}
		catch
		{
			sQLiteConnection.Close();
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
		SQLiteCommand sQLiteCommand = new SQLiteCommand
		{
			Connection = (SQLiteConnection)idbConnection_0,
			CommandText = string_0,
			CommandType = CommandType.Text
		};
		if (idataParameter_0 != null)
		{
			foreach (IDataParameter value in idataParameter_0)
			{
				sQLiteCommand.Parameters.Add(value);
			}
		}
		object result = sQLiteCommand.ExecuteScalar();
		sQLiteCommand.Parameters.Clear();
		return result;
	}

	public static object ExecuteScalar(string string_0, string string_1, params IDataParameter[] idataParameter_0)
	{
		using SQLiteConnection sQLiteConnection = new SQLiteConnection(string_0);
		sQLiteConnection.Open();
		return ExecuteScalar(sQLiteConnection, string_1, idataParameter_0);
	}

	private static IDataReader smethod_0(IDbConnection idbConnection_0, SQLiteTransaction sqliteTransaction_0, string string_0, IDataParameter[] idataParameter_0, bool bool_0)
	{
		SQLiteCommand sQLiteCommand = new SQLiteCommand
		{
			Connection = (SQLiteConnection)idbConnection_0,
			Transaction = sqliteTransaction_0,
			CommandText = string_0,
			CommandType = CommandType.Text
		};
		if (idataParameter_0 != null)
		{
			foreach (IDataParameter value in idataParameter_0)
			{
				sQLiteCommand.Parameters.Add(value);
			}
		}
		IDataReader result = ((!bool_0) ? sQLiteCommand.ExecuteReader(CommandBehavior.CloseConnection) : sQLiteCommand.ExecuteReader());
		sQLiteCommand.Parameters.Clear();
		return result;
	}

	public static void UpdateDataSet(string string_0, string string_1, DataSet dataSet_0, string string_2)
	{
		SQLiteConnection sQLiteConnection = new SQLiteConnection(string_0);
		sQLiteConnection.Open();
		new SQLiteDataAdapter(string_1, sQLiteConnection).Update(dataSet_0, string_2);
		sQLiteConnection.Close();
	}
}
