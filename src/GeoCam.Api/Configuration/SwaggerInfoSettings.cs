using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace GeoCam.Api.Configuration
{
	public class SwaggerInfoSettings
	{
		public string Title { get; set; }

		public string Description { get; set; }

		public class ContactInfo
		{
			public string Name { get; set; }

			public string Email { get; set; }
		}

		public ContactInfo Contact { get; set; }

		public OpenApiInfo ToApiInfo(ApiVersionDescription apiDescription = null)
		{
			var apiInfo = new OpenApiInfo()
				{
					Title = Title,
					Description = Description,
				};

			if (Contact != null)
			{
				apiInfo.Contact = new OpenApiContact()
					{
						Name = Contact.Name,
						Email = Contact.Email
					};
			}

			if (apiDescription != null)
			{
				apiInfo.Version = apiDescription.ApiVersion.ToString();
			}

			return apiInfo;
		}
	}
}
