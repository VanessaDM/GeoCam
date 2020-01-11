using GeoCam.Api.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoCam.Api.Controllers.Cameras
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class CamerasController : Controller
	{
		public CamerasController(ILogger<CamerasController> logger)
		{
			_logger = logger ?? throw new ArgumentNullException($"'{nameof(logger)}' cannot be null.");
		}

		/// <summary>
		/// Returns all cameras that match the requested criteria
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="searchModel">Camera filter criteria</param>
		/// <returns>List of cameras that match the parameters</returns>
		[HttpPost("search")]
		[Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Search([FromServices] GeoCamDbContext dbContext, [FromBody] Models.CameraSearchModel searchModel)
		{
			// *TODO: hacky - validate model + move to service + don't rely on name being the only criteria param + use automapper for the DTO
			ICollection<Camera> cameras = null;
			if (!String.IsNullOrEmpty(searchModel?.Name))
				cameras = await dbContext.Cameras.Where(c => EF.Functions.Like(c.Name, $"%{searchModel.Name}%")).ToListAsync();
			else
				cameras = await dbContext.Cameras.ToListAsync();

			return Ok(cameras.Select(c => new Models.CameraModel() { Name = c.Name, Number = c.Number, Longitude = c.Longitude, Latitude = c.Latitude }));
		}

		#region Fields

		protected readonly ILogger _logger;

		#endregion
	}
}
