using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace GeoCam.Web.Components
{
	public class CameraTableBase : ComponentBase
	{
		public CameraTableBase()
		{
		}

		#region Parameters

		[Parameter]
		public string Id { get; set; }

		[Parameter]
		public string Title { get; set; }

		[Parameter]
		public IEnumerable<Api.Models.CameraModel> Cameras { get; set; }

		#endregion
	}
}
