
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Mapster;

namespace GameServerAPI;

public class GameRoundService : IGameRoundService
{
    IUnitOfWork _unitOfWork;

    public GameRoundService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a random round
    /// </summary>
    /// <returns>An instance of a GameRoundDto object</returns>
    public async Task<GameRoundDto> CreateRound()
    {
        //Create new round and insert in the database
        var round = GameHelper.CreateNewRound();
        await _unitOfWork.GameRounds.Insert(round);
        await _unitOfWork.SaveChangesAsync();

        //Create player round for all online players in order to track their answer later on
        var onlinePlayers = await _unitOfWork.Players.GetByConnectionId(GameServerHub.ConnectedIds);
        foreach(var playerId in onlinePlayers.Select(x => x.Id))
        {
            var playerRound = new PlayerRound
            {   
                RoundId = round.Id,
                DateCreated = DateTime.Now,
                PlayerId = playerId
            };
            await _unitOfWork.PlayerRound.Insert(playerRound);
        }
        await _unitOfWork.SaveChangesAsync();
        var roundDto = round.Adapt<GameRoundDto>();
        return roundDto;
    }

    /// <summary>
    /// Returns the last round
    /// </summary>
    /// <returns>An instance of a GameRoundDto object</returns>
    public async Task<GameRoundDto> GetLastRound()
    {
        var lastRound = await _unitOfWork.GameRounds.GetLastRound();
        if(lastRound == null){
            return await CreateRound();
        }
        var roundDto = lastRound.Adapt<GameRoundDto>();
        return roundDto;
    }

    /// <summary>
    /// Check if the answer is correct
    /// </summary>
    /// <param name="roundId">Round Id</param>
    /// <param name="playerId"> Player Id</param>
    /// <param name="answer">The answer</param>
    /// <returns>true if the answer is correct and false otherwise</returns>
    public async Task<bool> SubmitAnswer(int roundId, Guid playerId, string answer)
    {
        var round = await _unitOfWork.GameRounds.GetById(roundId);
        if(round == null){
            throw new Exception("Round not found");
        }
        var player = await _unitOfWork.Players.GetById(playerId);
        if(player == null){
            throw new Exception("Player not found");
        }

        if(answer == round.CorrectAnswer){
            round.AnsweredByPlayerId = playerId;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
