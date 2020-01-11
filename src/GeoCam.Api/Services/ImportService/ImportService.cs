using CsvHelper;
using GeoCam.Api.DataAccess;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeoCam.Api.Services.ImportService
{
	public class ImportService : IImportService
	{
		public ImportService(GeoCamDbContext dbContext, ILogger<ImportService> logger)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException($"{nameof(dbContext)} cannot be null.");
			_logger = logger ?? throw new ArgumentNullException($"{nameof(logger)} cannot be null.");
		}

		public async Task<Models.ImportResult> ImportCamerasAsync(Stream inputStream, Encoding inputEncoding, bool partialImport = false)
		{
			var importResult = new Models.ImportResult();

			using var streamReader = new StreamReader(inputStream, inputEncoding);
			using var csvReader = new CsvReader(streamReader, new CsvHelper.Configuration.Configuration()
				{
					Delimiter = ";",
				});

			while (csvReader.Read())
			{
				Models.CameraCsvImportModel camera = null;
				try
				{
					camera = csvReader.GetRecord<Models.CameraCsvImportModel>();
				}
				catch (Exception ex) when (ex is CsvHelper.MissingFieldException || ex is CsvHelper.BadDataException)
				{
					importResult.DataErrors.Add($"[Row {csvReader.Context.Row}]: {csvReader.Context.RawRecord}");
					importResult.Rows.Errors++;

					if (!partialImport)
					{
						importResult.IsComplete = false;
						importResult.Rows = null; // Wipe out the row results
						return importResult;
					}
					continue;
				}

				// *TODO: refactor with yield  + use automapper for model mapping + better insert conflict handling
				try
				{
					await _dbContext.Cameras.AddAsync(new Camera() {
						Name = camera.Camera,
						Number = GetCameraNumberFromName(camera.Camera),
						Longitude = camera.Longitude,
						Latitude = camera.Latitude
					});
					await _dbContext.SaveChangesAsync();
				}
				catch (Microsoft.EntityFrameworkCore.DbUpdateException ex) when (ex.InnerException is Microsoft.Data.Sqlite.SqliteException innerEx && innerEx.SqliteErrorCode == 19)
				{
					// SQLITE_CONSTRAINT - Should be a unique constraint validation on long/lat aka a dupe
					importResult.Rows.Skipped++;
					continue;
				}
				importResult.Rows.Added++;
			}

			importResult.IsComplete = true;
			return importResult;
		}

		#region Helper functions

		// *TODO: unit test
		protected static int? GetCameraNumberFromName(string cameraName)
		{
			var rgxMatch = _rgxCameraNumber.Match(cameraName);
			if (!rgxMatch.Success)
				return null;

			var cameraNumberString = rgxMatch.Groups["Number"]?.Value;
			if (!Int32.TryParse(cameraNumberString, out int cameraNumber))
				return null;
			return cameraNumber;
		}

		#endregion

		#region Fields

		protected readonly GeoCamDbContext _dbContext;
		protected readonly ILogger _logger;
		protected static readonly Regex _rgxCameraNumber = new Regex(rgxCameraNumberPattern, RegexOptions.Compiled | RegexOptions.Singleline);

		#endregion

		#region Constants

		protected const string rgxCameraNumberPattern = @"^[A-Z]{3}-[A-Z]{2}-(?<Number>[0-9]{1,})";

		#endregion
	}
}
