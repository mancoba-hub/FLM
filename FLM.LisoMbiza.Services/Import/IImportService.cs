using System.IO;
using System.Threading.Tasks;

namespace FLM.LisoMbiza
{
    public interface IImportService
    {
        Task ImportBranchList(Stream stream, string contentType);

        Task ImportProductList(Stream stream, string contentType);

        Task ImportBranchProductList(Stream stream, string contentType);
    }
}
