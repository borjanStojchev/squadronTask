using Domain.Entities;

namespace Domain.Interfaces;

public interface IPlayerRepository : IBaseRepository<Player>
{ 
    Task<IEnumerable<Player>> GetByConnectionId(IEnumerable<string> connectionIds);
    Task<Player?> GetByConnectionId(string id);
}
