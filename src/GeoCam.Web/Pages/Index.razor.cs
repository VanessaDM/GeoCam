using GeoCam.Api.Clients;
using Microsoft.AspNetCore.Components;
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
		}

		#region Properties

		[Inject]
		protected ICameraClient CameraClient { get; set; }

		protected List<Api.Models.CameraModel> Cameras { get; set; } = new List<Api.Models.CameraModel>();

		#endregion
	}
}
