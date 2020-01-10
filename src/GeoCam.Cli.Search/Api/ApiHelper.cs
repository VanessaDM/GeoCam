using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GeoCam.Cli.Search.Api
{
	public static class ApiHelper
	{
		public static async Task<TResponse> JsonRequestAsync<TRequest, TResponse>(HttpMethod httpMethod, string requestUri, HttpClient httpClient, TRequest requestData,
																				  JsonSerializerSettings jsonSerializerSettings,
																				  Func<HttpResponseMessage, Task> nonSuccessHandler = null)
		{
			using var apiRequest = new HttpRequestMessage(httpMethod, requestUri);
			if (requestData != null)
			{
				apiRequest.Content = new ObjectContent<TRequest>(requestData,
																	new JsonMediaTypeFormatter() { SerializerSettings = jsonSerializerSettings },
																	new MediaTypeWithQualityHeaderValue(System.Net.Mime.MediaTypeNames.Application.Json));
			}

			using (var apiResponse = await httpClient.SendAsync(apiRequest, HttpCompletionOption.ResponseHeadersRead))
			{
				if (apiResponse.IsSuccessStatusCode)
					return await apiResponse.Content.ReadAsAsync<TResponse>();

				if (nonSuccessHandler != null)
					await nonSuccessHandler(apiResponse);
				throw new HttpRequestException();
			}
		}
	}
}
