using DataAccess.Repositories;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class PlayerRoundRepository : BaseRepository<PlayerRound>, IPlayerRoundRepository
{

    public PlayerRoundRepository(ApplicationContext context):base(context)
    {
    }

    public async Task<PlayerRound> GetLog(int roundId, Guid playerId)
    {
        return await _context.PlayerRound.FirstOrDefaultAsync(x => x.RoundId == roundId && x.PlayerId == playerId);
    }

    public async Task<IEnumerable<PlayerRound>> GetLogs(int roundId, IEnumerable<Guid> playerIds)
    {
        return await _context.PlayerRound.Where(x => x.RoundId == roundId && playerIds.Contains(x.PlayerId)).ToListAsync();
    }

    public async Task Insert(IEnumerable<PlayerRound> playerRounds)
    {
        await _context.PlayerRound.AddRangeAsync(playerRounds);
    }
}
