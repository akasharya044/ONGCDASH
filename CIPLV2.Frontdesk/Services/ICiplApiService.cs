using CIPLV2.Models.Admin;

namespace CIPLV2.Frontdesk.Services
{
    public interface ICiplApiService
    {
        Task<Response> GetData(string endpoint);
        Task<Response> PostData(object input, string endpoint);
        Task<Stream> GetStreamData(object input, string endpoint);
    }
}
