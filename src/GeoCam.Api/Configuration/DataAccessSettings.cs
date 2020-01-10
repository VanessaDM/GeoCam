using Microsoft.Data.Sqlite;

namespace GeoCam.Api.Configuration
{
	public class DataAccessSettings
	{
		public string DataSource { get; set; }

		public string ToConnectionString()
		{
			var connStringBuilder = new SqliteConnectionStringBuilder
				{
					DataSource = DataSource
				};
			return connStringBuilder.ToString();
		}
	}
}
