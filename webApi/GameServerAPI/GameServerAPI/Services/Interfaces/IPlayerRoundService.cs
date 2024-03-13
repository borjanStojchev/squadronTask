namespace GameServerAPI;

public interface IPlayerRoundService
{
    Task<bool> AddAnswer(Guid playerId, int roundId, string answer);
    Task<bool> IsRoundOver(int roundId, List<string> connectionIds);
}
