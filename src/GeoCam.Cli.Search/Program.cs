using GeoCam.Api.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GeoCam.Cli.Search
{
	class Program
	{
		/// <summary>
		/// GeoCam - CLI Demo Application
		/// </summary>
		/// <param name="name">Word or phrase to search for in the camera description</param>
		/// <param name="apiEndpoint">Base endpoint URI for the web API (e.g. http://geoapi.example.com/api/v1)</param>
		private static async Task<int> Main(string name, string apiEndpoint)
		{
			//
			// Configuration
			//
			IConfiguration configuration = new ConfigurationBuilder()
					.AddJsonFile("appsettings.json")
					.Build();

			var appSettings = configuration.Get<Configuration.AppSettings>();

			// *TODO: handle this more cleanly
			if (!String.IsNullOrEmpty(apiEndpoint))
			{
				appSettings.ApiEndpoint = apiEndpoint;
			}

			if (!Uri.IsWellFormedUriString(appSettings.ApiEndpoint, UriKind.Absolute))
			{
				Console.WriteLine("Invalid API endpoint configuration.");
				return -1;
			}

			//
			// API call
			//
			List<CameraModel> searchResults = null;
			{
				using HttpClientHandler httpClientHandler = new HttpClientHandler();
//#if DEBUG
				// Don't perform SSL validation on localhost addresses
				httpClientHandler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
					{
						return (sender.RequestUri.IsLoopback) ? true : System.Net.Security.SslPolicyErrors.None == sslPolicyErrors;
					};
//#endif // DEBUG

				using (var httpClient = new HttpClient(httpClientHandler))
				{
					try
					{
						var cameraClient = new Api.CameraClient(Microsoft.Extensions.Options.Options.Create(appSettings), httpClient);
						searchResults = await cameraClient.SearchAsync(new CameraSearchModel() { Name = name });
					}
					catch (Exception ex)
					{
						// *TODO: better error reporting
						Console.WriteLine($"An error occurred while calling the camera API: {ex.GetType().Name} - {ex.Message}");
						//Console.WriteLine($"An error occurred while calling the camera API:\r\n{ex.ToString()}");
						return -1;
					}
				}
			}

			//
			// Output results
			//
			if (searchResults.Count > 0)
			{
				foreach (var line in FormatSearchResults(searchResults))
				{
					Console.WriteLine(line);
				}
			}
			else
			{
				Console.WriteLine("No results matched your search criteria.");
			}

			return 0;
		}

		#region Helper functions

		protected static IEnumerable<string> FormatSearchResults(ICollection<CameraModel> searchResults)
		{
			// *TODO: first column of expected output still missing - extract with regular expression (or supposed to be db id?)
			Func<CameraModel, string>[] selectors = new Func<CameraModel, string>[]
				{
					x => x.Name,
					x => x.Latitude.ToString(),
					x => x.Longitude.ToString(),
				};
			foreach (var camera in searchResults)
			{
				yield return String.Join(" | ", selectors.Select(x => x(camera)));
			}
		}

		#endregion
	}
}
