using BBCAD.API.DTO;

namespace BBCAD.Site.Services
{
    public interface IBatchProcessingService
    {
        Task<BatchProcessingResponce> CallBoardAPI(HttpMethod method, string requestStr);
    }
}
