using Domain.Entities;
using Domain.Interfaces;

namespace Domain;

public interface IPlayerRoundRepository : IBaseRepository<PlayerRound>
{
    Task Insert(IEnumerable<PlayerRound> playerRounds);
    Task<IEnumerable<PlayerRound>> GetLogs(int roundId, IEnumerable<Guid> playerIds);
    Task<PlayerRound> GetLog(int roundId, Guid playerId);
}
