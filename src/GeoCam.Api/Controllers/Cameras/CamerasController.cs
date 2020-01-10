using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace GeoCam.Api.Controllers.Cameras
{
	[ApiController]
	[ApiVersion("1")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class CamerasController : Controller
	{
		public CamerasController(ILogger<CamerasController> logger)
		{
			_logger = logger ?? throw new ArgumentNullException($"'{nameof(logger)}' cannot be null.");
		}

		[HttpPost("search")]
		[Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(void), StatusCodes.Status501NotImplemented)]
		public IActionResult Search([FromBody] Models.CameraSearchModel searchModel)
		{
			throw new NotImplementedException();
		}

		#region Fields

		protected readonly ILogger _logger;

		#endregion
	}
}
