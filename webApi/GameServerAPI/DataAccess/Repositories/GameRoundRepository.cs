using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class GameRoundRepository : BaseRepository<GameRound>, IGameRoundRepository
{
    public GameRoundRepository(ApplicationContext context):base(context)
    {
    }

    public async Task<GameRound?> GetLastRound()
    {
        return await _context.GameRound.OrderBy(x=>x.Id).LastOrDefaultAsync();
    }
}
