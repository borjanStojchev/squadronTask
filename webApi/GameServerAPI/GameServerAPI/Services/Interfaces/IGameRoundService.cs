namespace GameServerAPI;

public interface IGameRoundService
{
    Task<GameRoundDto> GetLastRound();
    Task<GameRoundDto> CreateRound();
    Task<bool> SubmitAnswer(int roundId, Guid playerId, string asnwer);
}
