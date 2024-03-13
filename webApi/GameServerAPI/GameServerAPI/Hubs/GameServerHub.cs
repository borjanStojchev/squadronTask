using Microsoft.AspNetCore.SignalR;

namespace GameServerAPI;

public class GameServerHub : Hub
{
    public static List<string> ConnectedIds = new List<string>();
    private IGameRoundService _gameRoundService;
    private IPlayerService _playerService;

    public GameServerHub(IPlayerService playerService, IGameRoundService gameRoundService)
    {
        _playerService = playerService;
        _gameRoundService = gameRoundService;
    }
    
    public async override Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        //disconnect the player if there are already 5 players
        if(GameServerHub.ConnectedIds.Count == 5) 
        {
            Context.Abort();
            return;
        }
        //Create the player and get the last round
        var player = await _playerService.Create(Context.ConnectionId);
        var round = await _gameRoundService.GetLastRound();
        //Notify the player that the player is created and also notify the player of the last round
        await Clients.Caller.SendAsync("playerCreated",player);
        await Clients.Caller.SendAsync("firstRound", round);
        //Notify all the players of the number of players online
        await Clients.All.SendAsync("playersCountUpdate", GameServerHub.ConnectedIds.Count + 1);
        GameServerHub.ConnectedIds.Add(Context.ConnectionId);
    }

    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        //Notify all the players of the number of players online
        await Clients.All.SendAsync("playersCountUpdate", GameServerHub.ConnectedIds.Count - 1);
        GameServerHub.ConnectedIds.Remove(Context.ConnectionId);
    }
}
