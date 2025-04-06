using MessagePack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ServerAPI.DB;
using ServerAPI.Models;
using StackExchange.Redis;
using System.Diagnostics;
using Newtonsoft.Json;
using ServerAPI.Entities;

namespace ServerAPI.Services
{
    public class RankingService : IRankingService
    {
        private readonly IDistributedCache _cache;
        private readonly AppDbContext _context;

        public RankingService(IDistributedCache cache, AppDbContext context)
        {
            _cache = cache; 
            _context = context;
        }

        public async Task<List<RankingDto>> GetTopRankingsAsync()
        {
            string cacheKey = "ranking:top10";
         
            string cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
            {
                //Console.WriteLine($"Cache hit for key: {cacheKey}, value: {cached}");
                return JsonConvert.DeserializeObject<List<RankingDto>>(cached);
            }

            //Console.WriteLine($"Cache miss for key: {cacheKey}");
            var rankings = await _context.Rankings
                            .OrderBy(r => r.Rank)
                            .Take(10)
                            .Select(r => new RankingDto
                            {
                                Rank = r.Rank,
                                Username = r.Username,
                                Level = r.Level
                            })
                            .ToListAsync();

            string serialized = JsonConvert.SerializeObject(rankings);
            await _cache.SetStringAsync(cacheKey, serialized, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            //Console.WriteLine($"Cache set for key: {cacheKey}, value: {serialized}");

            return rankings;
        }

        public async Task<RankingDto> GetMyRankingAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("User ID cannot be null or empty");

            var ranking = await _context.Rankings
                .FirstOrDefaultAsync(r => r.UserId == int.Parse(userId));
            if (ranking == null) throw new Exception("Don't have Rank");

            return new RankingDto
            {
                Rank = ranking.Rank,
                Username = ranking.Username,
                Level = ranking.Level
            };
        }

        public async Task UpdateRankingsAsync()
        {
            _context.Rankings.RemoveRange(_context.Rankings);

            var users = await _context.Users
                .OrderByDescending(u => u.Level)
                .ToListAsync();

            var rankings = users.Select((u, index) => new Ranking
            {
                UserId = u.Id,
                Username = u.Username,
                Level = u.Level,
                Rank = index + 1,
                LastUpdated = DateTime.UtcNow
            }).ToList();

            await _context.Rankings.AddRangeAsync(rankings);
            await _context.SaveChangesAsync();
        }

    }
}
