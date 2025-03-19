using ServerAPI.Models;

namespace ServerAPI.Services
{
    public interface IRankingService
    {
        Task<List<RankingDto>> GetTopRankingsAsync();
        Task<RankingDto> GetMyRankingAsync(string userId);
        Task UpdateRankingsAsync();
    }
}
