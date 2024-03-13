using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GameServerAPI;

[ApiController]
[Route("[controller]")]
public class GameRoundController : ControllerBase
{
    private readonly IGameRoundService _gameRoundService;
    private readonly IPlayerRoundService _playerRoundService;
    protected readonly IHubContext<GameServerHub> _hubContext;

    public GameRoundController(IGameRoundService gameRoundService, IHubContext<GameServerHub> hubContext, IPlayerRoundService playerRoundLogService)
    {
        _gameRoundService = gameRoundService;
        _playerRoundService = playerRoundLogService;
        _hubContext = hubContext;
    }
    /// <summary>
    /// This endpoint is used when the player need to submit the answer.
    /// </summary>
    /// <param name="roundId">The round which the player is submiting the answer</param>
    /// <param name="playerId">The player Id </param>
    /// <param name="answer">The answer</param>
    /// <returns></returns>
    [HttpGet("submitAnswer")]
    public async Task<IActionResult> SubmitAnswer([FromQuery] int roundId, Guid playerId, string answer)
    {
        //Check if the answer is correct
        var isAnswerCorrect = await _gameRoundService.SubmitAnswer(roundId, playerId, answer);
        //Add the answer to the playerRound table in order to keep track of all the answers
        await _playerRoundService.AddAnswer(playerId,roundId,answer);
        if(isAnswerCorrect)
        {
            //if the answer is correct we create new Round and notify all the players
            var round = await _gameRoundService.CreateRound();
            await _hubContext.Clients.All.SendAsync("newRound", round);
        }
        else
        {
            //if the answer is not correct we need to check if all players had submit an incorrect answer and start a new round
            var connectionIds = GameServerHub.ConnectedIds;
            var isRoundOver = await _playerRoundService.IsRoundOver(roundId, connectionIds);
            if(isRoundOver)
            {
                var round = await _gameRoundService.CreateRound();
                await _hubContext.Clients.All.SendAsync("newRound", round);
            }
        }
        return Ok(isAnswerCorrect);
    }
}
