using DataAccess.Repositories;
using Domain;
using Domain.Interfaces;

namespace DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContext _dbContext;
    private readonly Lazy<IPlayerRepository> _lazyPlayerRepository;  
    private readonly Lazy<IGameRoundRepository> _lazyGameRoundRepository;
    private readonly Lazy<IPlayerRoundRepository> _lazyPlayerRoundRepository;

    public UnitOfWork(ApplicationContext context)
    {
        _dbContext = context;
        _lazyPlayerRepository = new Lazy<IPlayerRepository>(() => new PlayerRepository(context));
        _lazyGameRoundRepository = new Lazy<IGameRoundRepository>(() => new GameRoundRepository(context));
        _lazyPlayerRoundRepository = new Lazy<IPlayerRoundRepository>(() => new PlayerRoundRepository(context));
    }
    public IPlayerRepository Players => _lazyPlayerRepository.Value;
    public IGameRoundRepository GameRounds => _lazyGameRoundRepository.Value;
    public IPlayerRoundRepository PlayerRound => _lazyPlayerRoundRepository.Value;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
