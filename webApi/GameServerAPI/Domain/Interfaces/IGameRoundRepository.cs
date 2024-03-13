using Domain.Entities;

namespace Domain.Interfaces;

public interface IGameRoundRepository : IBaseRepository<GameRound>
{
    Task<GameRound?> GetLastRound();

}
