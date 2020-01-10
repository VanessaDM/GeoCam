using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace GeoCam.Api.Configuration
{
	public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
	{
		public ConfigureSwaggerOptions(IApiVersionDescriptionProvider versionProvider, IOptions<SwaggerInfoSettings> swaggerInfoSettings)
		{
			_versionProvider = versionProvider ?? throw new ArgumentNullException($"{nameof(versionProvider)} cannot be null.");
			_swaggerInfoSettings = swaggerInfoSettings?.Value ?? throw new ArgumentNullException($"{nameof(swaggerInfoSettings)} cannot be null."); ;
		}

		#region IConfigureOptions implementation

		public void Configure(SwaggerGenOptions options)
		{
			// Add a swagger document for each discovered API version
			foreach (var description in _versionProvider.ApiVersionDescriptions)
			{
				options.SwaggerDoc(description.GroupName, _swaggerInfoSettings.ToApiInfo(description));
			}
		}

		#endregion IConfigureOptions implementation

		#region Fields

		protected readonly IApiVersionDescriptionProvider _versionProvider;

		protected readonly SwaggerInfoSettings _swaggerInfoSettings;

		#endregion Fields
	}
}
