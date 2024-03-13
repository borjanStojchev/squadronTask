using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class PlayerRepository : BaseRepository<Player>, IPlayerRepository
{
    public PlayerRepository(ApplicationContext context):base(context)
    {
    }

    public async Task<Player?> GetByConnectionId(string id)
    {
        return await _context.Player.FirstOrDefaultAsync(x=>x.ConnectionId == id);
    }

    public async Task<IEnumerable<Player>> GetByConnectionId(IEnumerable<string> connectionIds)
    {
        return await _context.Player.Where(x=>connectionIds.Contains(x.ConnectionId)).ToListAsync();
    }
}
