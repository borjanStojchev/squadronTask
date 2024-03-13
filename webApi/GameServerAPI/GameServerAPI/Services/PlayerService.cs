using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Mapster;

namespace GameServerAPI;

public class PlayerService : IPlayerService
{
    IUnitOfWork _unitOfWork;

    public PlayerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Create a new player
    /// </summary>
    /// <param name="connectionId">The connection id of the player</param>
    /// <returns>The new player</returns>
    public async Task<PlayerDto?> Create(string connectionId){

        var player = new Player(){
            Id = Guid.NewGuid(),
            ConnectionId = connectionId,
            DateCreated = DateTime.Now
        };
        await _unitOfWork.Players.Insert(player);

        //Insert the player in the last round so he can submit an answer 
        var round = await _unitOfWork.GameRounds.GetLastRound();
        if(round == null){
            //If there is no rounds available, a new round should be created
            round = GameHelper.CreateNewRound();
            await _unitOfWork.GameRounds.Insert(round);
            await _unitOfWork.SaveChangesAsync();
        }
        var playerRound = new PlayerRound
        {
            PlayerId = player.Id,
            RoundId = round.Id,
            DateCreated = DateTime.Now
        };
        await _unitOfWork.PlayerRound.Insert(playerRound);

        await _unitOfWork.SaveChangesAsync();
        var playerDto = player.Adapt<PlayerDto>();
        return playerDto;
    }
}
