using System.Collections.Generic;

namespace GeoCam.Api.Services.ImportService.Models
{
	public class ImportResult
	{
		public bool IsComplete { get; set; } = false;

		public class RowResults
		{
			public int Added { get; set; } = 0;

			public int Skipped { get; set; } = 0;

			public int Errors { get; set; } = 0;
		}
		public RowResults Rows { get; set; } = new RowResults();

		public List<string> DataErrors { get; } = new List<string>();
	}
}
