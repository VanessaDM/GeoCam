using GeoCam.Api.Services.ImportService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GeoCam.Api.Controllers.Import
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class ImportController : Controller
	{
		public ImportController(ILogger<ImportController> logger)
		{
			_logger = logger ?? throw new ArgumentNullException($"{nameof(logger)} cannot be null.");
		}

		/// <summary>
		/// Import cameras using a CSV file (anonymous access for demo purposes only)
		/// </summary>
		/// <param name="importService"></param>
		/// <param name="csvFile">CSV file containing the cameras to be imported (UTF-8 encoding)</param>
		/// <param name="partialImport">Specifies whether to perform a rollback on data errors, or import as much as possible</param>
		/// <returns></returns>
		//[Authorize(...)]
		[HttpPost("cameras/csvfile")]
		[Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(Services.ImportService.Models.ImportResult), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ImportCsv([FromServices] IImportService importService, IFormFile csvFile, [FromQuery] bool partialImport = false)
		{
			using var importStream = csvFile.OpenReadStream();

			var importResult = await importService.ImportCamerasAsync(importStream, System.Text.Encoding.UTF8, partialImport);
			return Ok(importResult);
		}

		#region Fields

		protected readonly ILogger _logger;

		#endregion Fields
	}
}
