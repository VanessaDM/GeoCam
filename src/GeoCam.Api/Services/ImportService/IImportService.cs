using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GeoCam.Api.Services.ImportService
{
	public interface IImportService
	{
		Task<Models.ImportResult> ImportCamerasAsync(Stream inputStream, Encoding inputEncoding, bool partialImport = false);
	}
}
