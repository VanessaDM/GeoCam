using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace GeoCam.Api.Startup
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			//
			// Configuration
			//
			var appSettings = Configuration.GetSection("App").Get<Configuration.AppSettings>();
			services
				.Configure<Configuration.AppSettings>(Configuration.GetSection("App"))
				.Configure<Configuration.SwaggerInfoSettings>(Configuration.GetSection("Swagger"));

			//
			// API + versioning
			//
			services
				.AddControllers();

			services
				.AddApiVersioning(options =>
					{
						options.ReportApiVersions = true;
					})
				.AddVersionedApiExplorer(options =>
					{
						// Format version as 'v'major[.minor][-status]
						options.GroupNameFormat = "'v'VVV";

						options.SubstituteApiVersionInUrl = true;
					});

			//
			// Data access
			//
			var dataAccessSettings = Configuration.GetSection("DataAccess").Get<Configuration.DataAccessSettings>();
			services
				.AddDbContext<DataAccess.GeoCamDbContext>(options =>
					{
						options.UseSqlite(dataAccessSettings.ToConnectionString(), sqlLiteOptions => sqlLiteOptions.MigrationsAssembly("GeoCam.Api.Migrations"));
					});

			//
			// Dependency Injection
			//

			//
			// Swagger
			//
			if (appSettings.EnableSwagger)
			{
				services.AddTransient<IConfigureOptions<Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions>, Configuration.ConfigureSwaggerOptions>();
				services.AddSwaggerGen(options =>
					{
						// Set the comments path for the Swagger JSON and UI
						var xmlFile = $"{typeof(Startup).GetTypeInfo().Assembly.GetName().Name}.xml";
						var xmlPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, xmlFile);
						options.IncludeXmlComments(xmlPath);

						// Use full schema names to avoid v1/v2/v3 schema collisions see https://github.com/domaindrivendev/Swashbuckle/issues/442
						options.CustomSchemaIds(type => type.FullName);
					});
			}
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<Configuration.AppSettings> appSettings, IApiVersionDescriptionProvider provider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
				{
					endpoints.MapControllers();
				});

			//
			// Swagger
			//
			if (appSettings.Value.EnableSwagger)
			{
				app.UseSwagger(options =>
					{
					});
				app.UseSwaggerUI(options =>
					{
						// Build a swagger endpoint for each discovered API version
						foreach (var description in provider.ApiVersionDescriptions)
							options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
						options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
						options.DisplayRequestDuration();
						options.EnableDeepLinking();
						options.EnableFilter();
					});
			}
		}

		#region Properties

		public IConfiguration Configuration { get; }

		#endregion
	}
}
