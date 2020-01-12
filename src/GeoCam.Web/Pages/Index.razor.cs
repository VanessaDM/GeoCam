using GeoCam.Api.Clients;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoCam.Web.Pages
{
	public class IndexBase : ComponentBase
	{
		public IndexBase()
		{
		}

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			// *TODO: error handling
			Cameras = await CameraClient.SearchAsync(new Api.Models.CameraSearchModel() { Name = null });
			await JSRuntime.InvokeAsync<Task>("googleMap.addMarkers", Cameras);
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			await JSRuntime.InvokeAsync<Task>("initGoogleMapBlazor");
		}

		#region Properties

		[Inject]
		protected ICameraClient CameraClient { get; set; }

		protected List<Api.Models.CameraModel> Cameras { get; set; } = new List<Api.Models.CameraModel>();

		[Inject]
		protected IJSRuntime JSRuntime { get; set; }

		#endregion
	}
}
