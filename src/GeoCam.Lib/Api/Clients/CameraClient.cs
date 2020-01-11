using GeoCam.Api.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GeoCam.Api.Clients
{
	public class CameraClient : ICameraClient
	{
		public CameraClient(IOptions<ApiSettings> apiSettings, HttpClient httpClient)
		{
			_apiSettings = apiSettings?.Value ?? throw new ArgumentNullException($"{nameof(apiSettings)} cannot be null.");
			_httpClient = httpClient ?? throw new ArgumentNullException($"{nameof(httpClient)} cannot be null.");
		}

		public async Task<List<CameraModel>> SearchAsync(CameraSearchModel searchModel)
		{
			var camSearchUrl = UrlCombineLib.UrlCombine.Combine(_apiSettings.Endpoint, "/cameras/search");
			// *TODO: add a non-success handler + better error handling for our caller
			return await ApiHelper.JsonRequestAsync<CameraSearchModel, List<CameraModel>>(HttpMethod.Post, camSearchUrl, _httpClient, searchModel, _jsonSerializerSettings, null);
		}

		#region Fields

		protected readonly ApiSettings _apiSettings;
		protected readonly HttpClient _httpClient;
		protected readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings();

		#endregion
	}
}
