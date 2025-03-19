using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Services;
using System.Security.Claims;

namespace ServerAPI.Controllers
{
    [Route("api/ranking")]
    [ApiController]
    public class RankingController : ControllerBase
    {
        private readonly IRankingService _rankingService;

        public RankingController(IRankingService rankingService)
        {
            _rankingService = rankingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTopRankings()
        {
            try
            {
                var rankings = await _rankingService.GetTopRankingsAsync();
                return Ok(rankings);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize]
        [HttpGet("myrank")]
        public async Task<IActionResult> GetMyRanking()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var ranking = await _rankingService.GetMyRankingAsync(userId);
                return Ok(ranking);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return NotFound(e.Message);
            }
        }

        [HttpPost("updaterank")]
        public async Task<IActionResult> UpdateRankings()
        {
            try
            {
                await _rankingService.UpdateRankingsAsync();
                return Ok("Rankings updated");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
    }
}
