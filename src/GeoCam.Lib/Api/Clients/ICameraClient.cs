using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoCam.Api.Clients
{
	public interface ICameraClient
	{
		Task<List<Models.CameraModel>> SearchAsync(Models.CameraSearchModel searchModel);
	}
}
