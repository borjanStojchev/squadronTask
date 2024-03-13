
using Domain.Entities;
using Domain.Interfaces;

namespace GameServerAPI;

public class PlayerRoundService : IPlayerRoundService
{
    IUnitOfWork _unitOfWork;

    public PlayerRoundService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Check if the round has correct answer, if not check if all the online players subitted an answer
    /// </summary>
    /// <param name="roundId">Round Id</param>
    /// <param name="connectionIds">List of connection Ids of all the online players</param>
    /// <returns>a boolean indicating if the round is over</returns>
    /// <exception cref="Exception"></exception>
    public async Task<bool> IsRoundOver(int roundId, List<string> connectionIds)
    {
        //Get all online players 
        var onlinePlayers = await _unitOfWork.Players.GetByConnectionId(connectionIds);
        if(onlinePlayers == null || onlinePlayers.Count() == 0) return true;
        
        //Get the round
        var round = await _unitOfWork.GameRounds.GetById(roundId);
        if(round == null) throw new Exception("Round not Found");

        //Check if the round is already answered by a player
        if(round.AnsweredByPlayerId == null)
        {
            //Check if all players answered wrong
            var roundLogs = await _unitOfWork.PlayerRound.GetLogs(roundId, onlinePlayers.Select(x=>x.Id));
            return roundLogs.Count(x=>string.IsNullOrWhiteSpace(x.Answer)) == 0;      
        }
        return true;
    }

    /// <summary>
    /// Updating the row with an answer to track all the answers submitted
    /// </summary>
    /// <param name="playerId">PLayer Id</param>
    /// <param name="roundId">Round Id</param>
    /// <param name="answer">The answer</param>
    /// <returns></returns>
    public async Task<bool> AddAnswer(Guid playerId, int roundId, string answer)
    {
        var log = await _unitOfWork.PlayerRound.GetLog(roundId,playerId);
        log.Answer = answer;        
        log.DateExpressionAnswered = DateTime.Now;
        await _unitOfWork.PlayerRound.Update(log);
        var result = await _unitOfWork.SaveChangesAsync() == 1;
        return result;
    }
}
