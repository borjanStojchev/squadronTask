namespace Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
        IPlayerRepository Players { get; }
        IGameRoundRepository GameRounds { get; }
        IPlayerRoundRepository PlayerRound { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
