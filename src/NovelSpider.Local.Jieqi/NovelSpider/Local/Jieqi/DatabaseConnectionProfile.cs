using System;
using System.Data;
using System.Text;
using MySqlConnector;

namespace NovelSpider.Local.Jieqi;

public class DatabaseConnectionProfile
{
	public const string MySql = "MySQL";

	public const string MariaDb = "MariaDB";

	public const string PerconaServer = "Percona Server";

	public string ServerType { get; private set; }

	public string ServerVersion { get; private set; }

	public int MajorVersion { get; private set; }

	public string VersionComment { get; private set; }

	public string CharacterSetServer { get; private set; }

	public string CollationServer { get; private set; }

	public string SqlMode { get; private set; }

	public string OriginalConnectionString { get; private set; }

	public string RecommendedConnectionString { get; private set; }

	public bool ConnectionStringChanged { get; private set; }

	public string RecommendationMessage { get; private set; }

	private DatabaseConnectionProfile()
	{
	}

	public static DatabaseConnectionProfile Detect(string connectionString)
	{
		string safeConnectionString = NormalizeConnectionString(connectionString, MySql, 0);
		using MySqlConnection mySqlConnection = new MySqlConnection(safeConnectionString);
		using MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
		mySqlCommand.CommandType = CommandType.Text;
		mySqlCommand.CommandText = "SELECT VERSION(), @@version_comment, @@character_set_server, @@collation_server, @@sql_mode";
		mySqlConnection.Open();
		using MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader(CommandBehavior.SingleRow);
		if (!mySqlDataReader.Read())
		{
			throw new InvalidOperationException("数据库连接成功，但没有返回版本信息。");
		}
		string serverVersion = Convert.ToString(mySqlDataReader.GetValue(0));
		string versionComment = Convert.ToString(mySqlDataReader.GetValue(1));
		string characterSetServer = Convert.ToString(mySqlDataReader.GetValue(2));
		string collationServer = Convert.ToString(mySqlDataReader.GetValue(3));
		string sqlMode = Convert.ToString(mySqlDataReader.GetValue(4));
		return Create(safeConnectionString, serverVersion, versionComment, characterSetServer, collationServer, sqlMode);
	}

	public static string NormalizeConnectionString(string connectionString, string serverType, int majorVersion)
	{
		return ApplyRecommendedConnectionString(connectionString, NormalizeServerType(serverType), majorVersion, out _, out _);
	}

	public string ToDisplayText()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("数据库连接成功。");
		stringBuilder.AppendLine("服务器类型：" + ServerType);
		stringBuilder.AppendLine("服务器版本：" + ServerVersion);
		stringBuilder.AppendLine("版本说明：" + VersionComment);
		stringBuilder.AppendLine("服务器字符集：" + CharacterSetServer);
		stringBuilder.AppendLine("服务器排序规则：" + CollationServer);
		stringBuilder.AppendLine("SQL Mode：" + (string.IsNullOrEmpty(SqlMode) ? "空" : SqlMode));
		if (!string.IsNullOrEmpty(RecommendationMessage))
		{
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(RecommendationMessage);
		}
		return stringBuilder.ToString();
	}

	private static DatabaseConnectionProfile Create(string connectionString, string serverVersion, string versionComment, string characterSetServer, string collationServer, string sqlMode)
	{
		string serverType = DetectServerType(serverVersion, versionComment);
		int majorVersion = ParseMajorVersion(serverVersion);
		string recommendedConnectionString = ApplyRecommendedConnectionString(connectionString, serverType, majorVersion, out bool changed, out string recommendationMessage);
		return new DatabaseConnectionProfile
		{
			ServerType = serverType,
			ServerVersion = serverVersion ?? "",
			MajorVersion = majorVersion,
			VersionComment = versionComment ?? "",
			CharacterSetServer = characterSetServer ?? "",
			CollationServer = collationServer ?? "",
			SqlMode = sqlMode ?? "",
			OriginalConnectionString = connectionString,
			RecommendedConnectionString = recommendedConnectionString,
			ConnectionStringChanged = changed,
			RecommendationMessage = recommendationMessage
		};
	}

	private static string DetectServerType(string serverVersion, string versionComment)
	{
		string text = ((serverVersion ?? "") + " " + (versionComment ?? "")).ToLowerInvariant();
		if (text.Contains("mariadb"))
		{
			return MariaDb;
		}
		if (text.Contains("percona"))
		{
			return PerconaServer;
		}
		return MySql;
	}

	private static string NormalizeServerType(string serverType)
	{
		if (string.Equals(serverType, MariaDb, StringComparison.OrdinalIgnoreCase))
		{
			return MariaDb;
		}
		if (string.Equals(serverType, PerconaServer, StringComparison.OrdinalIgnoreCase))
		{
			return PerconaServer;
		}
		return MySql;
	}

	private static int ParseMajorVersion(string serverVersion)
	{
		if (string.IsNullOrEmpty(serverVersion))
		{
			return 0;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (char item in serverVersion)
		{
			if (!char.IsDigit(item))
			{
				break;
			}
			stringBuilder.Append(item);
		}
		return int.TryParse(stringBuilder.ToString(), out int result) ? result : 0;
	}

	private static string ApplyRecommendedConnectionString(string connectionString, string serverType, int majorVersion, out bool changed, out string recommendationMessage)
	{
		changed = false;
		recommendationMessage = "";
		if (string.IsNullOrWhiteSpace(connectionString))
		{
			return connectionString;
		}
		MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(connectionString);
		StringBuilder messageBuilder = new StringBuilder();
		if (serverType == MariaDb)
		{
			changed |= ForceCharacterSet(builder, "utf8mb4");
			changed |= SetSslModeIfMissing(builder, connectionString, MySqlSslMode.Preferred);
			if (changed)
			{
				messageBuilder.AppendLine("已按 MariaDB 推荐参数补齐连接串：Charset=utf8mb4;SslMode=Preferred。");
			}
		}
		else if (serverType == PerconaServer)
		{
			if (majorVersion >= 8)
			{
				changed |= ApplyMySql8OrNewerDefaults(builder);
				if (changed)
				{
					messageBuilder.AppendLine("已按 Percona Server 8.x 推荐参数补齐连接串：Charset=utf8mb4;SslMode=Preferred;AllowPublicKeyRetrieval=True。");
				}
			}
			else
			{
				messageBuilder.AppendLine("Percona Server 5.7.x 保留认证兼容策略，但写库字符集强制为 utf8mb4。");
			}
		}
		else if (majorVersion >= 8)
		{
			changed |= ApplyMySql8OrNewerDefaults(builder);
			if (changed)
			{
				messageBuilder.AppendLine("已按 MySQL 8/9 推荐参数优化：Charset=utf8mb4;SslMode=Preferred;AllowPublicKeyRetrieval=True。");
			}
		}
		else if (majorVersion <= 0)
		{
			changed |= ApplyUnknownMySqlDefaults(builder);
			if (changed)
			{
				messageBuilder.AppendLine("数据库版本未识别前，已补齐 MySQL 8/9 认证兼容参数：Charset=utf8mb4;SslMode=Preferred;AllowPublicKeyRetrieval=True。");
			}
		}
		else
		{
			messageBuilder.AppendLine("MySQL 5.7.x 保留认证兼容策略，但写库字符集强制为 utf8mb4。");
		}
		recommendationMessage = messageBuilder.ToString().TrimEnd();
		return changed ? builder.ConnectionString : connectionString;
	}

	private static bool ApplyMySql8OrNewerDefaults(MySqlConnectionStringBuilder builder)
	{
		bool changed = false;
		string connectionString = builder.ConnectionString;
		changed |= ForceCharacterSet(builder, "utf8mb4");
		changed |= SetSslModeIfMissing(builder, connectionString, MySqlSslMode.Preferred);
		changed |= SetAllowPublicKeyRetrievalIfMissing(builder, connectionString, true);
		return changed;
	}

	private static bool ApplyUnknownMySqlDefaults(MySqlConnectionStringBuilder builder)
	{
		bool changed = false;
		string connectionString = builder.ConnectionString;
		changed |= ForceCharacterSet(builder, "utf8mb4");
		changed |= SetSslModeIfMissing(builder, connectionString, MySqlSslMode.Preferred);
		changed |= SetAllowPublicKeyRetrievalIfMissing(builder, connectionString, true);
		return changed;
	}

	private static bool ForceCharacterSet(MySqlConnectionStringBuilder builder, string value)
	{
		if (string.Equals(builder.CharacterSet, value, StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		builder.CharacterSet = value;
		return true;
	}

	private static bool SetSslModeIfMissing(MySqlConnectionStringBuilder builder, string connectionString, MySqlSslMode value)
	{
		if (ContainsAnyKey(connectionString, "SSL Mode", "SslMode", "Ssl Mode"))
		{
			return false;
		}
		builder.SslMode = value;
		return true;
	}

	private static bool SetAllowPublicKeyRetrievalIfMissing(MySqlConnectionStringBuilder builder, string connectionString, bool value)
	{
		if (ContainsAnyKey(connectionString, "Allow Public Key Retrieval", "AllowPublicKeyRetrieval"))
		{
			return false;
		}
		builder.AllowPublicKeyRetrieval = value;
		return true;
	}

	private static bool ContainsAnyKey(string connectionString, params string[] keys)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			return false;
		}
		string[] segments = connectionString.Split(';');
		foreach (string segment in segments)
		{
			int equalsIndex = segment.IndexOf('=');
			if (equalsIndex <= 0)
			{
				continue;
			}
			string optionName = NormalizeOptionName(segment.Substring(0, equalsIndex));
			foreach (string key in keys)
			{
				if (optionName == NormalizeOptionName(key))
				{
					return true;
				}
			}
		}
		return false;
	}

	private static string NormalizeOptionName(string optionName)
	{
		return (optionName ?? "").Replace(" ", "").Replace("_", "").Replace("-", "").Trim().ToLowerInvariant();
	}
}


