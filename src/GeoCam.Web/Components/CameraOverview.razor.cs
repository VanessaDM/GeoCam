using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace GeoCam.Web.Components
{
	public class CameraOverviewBase : ComponentBase
	{
		public CameraOverviewBase()
		{
		}

		#region Parameters

		[Parameter]
		public IReadOnlyCollection<Api.Models.CameraModel> Cameras
		{
			get { return _cameras; }
			set
			{
				_cameraColumns = value.ToLookup(c =>
					{
						bool divBy3 = (c.Number.HasValue) ? (c.Number.Value % 3 == 0) : false;
						bool divBy5 = (c.Number.HasValue) ? (c.Number.Value % 5 == 0) : false;

						if (divBy3 && divBy5)
							return "column15";
						else if (divBy3)
							return "column3";
						else if (divBy5)
							return "column5";
						else
							return "columnOther";
					});
			}
		}

		#endregion

		#region Fields

		protected IReadOnlyCollection<Api.Models.CameraModel> _cameras = new List<Api.Models.CameraModel>();

		protected ILookup<string, Api.Models.CameraModel> _cameraColumns;

		#endregion
	}
}
