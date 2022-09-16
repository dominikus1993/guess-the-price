using GuessThePrice.Core.Grains;
using GuessThePrice.Core.Model;
using GuessThePrice.WebApp.Requests;
using GuessThePrice.WebApp.Responses;

using Microsoft.AspNetCore.Mvc;

using Orleans;

using ProductId = GuessThePrice.Core.Model.ProductId;

namespace GuessThePrice.WebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly IClusterClient _cluster;

    public GameController(IClusterClient cluster)
    {
        _cluster = cluster;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GameResponse>> Get(Guid id)
    {
        var gameGrain = _cluster.GetGrain<IGameGrain>(id);
        var game = await gameGrain.GetGame();
        var response = GameResponse.FromGame(game);
        return Ok(response);
    }

    [HttpGet("{id:guid}/score")]
    public async Task<ActionResult<ScoreResponse>> GetGameScore(Guid id)
    {
        var gameGrain = _cluster.GetGrain<IGameGrain>(id);
        var score = await gameGrain.GetGameScore();
        return Ok(new ScoreResponse() { Value = score.Value });
    }

    [HttpPost("{id:guid}")]
    public async Task<IActionResult> StartGame(Guid id)
    {
        var gameGrain = _cluster.GetGrain<IGameGrain>(id);
        var game = await gameGrain.StartGame();
        return Ok();
    }

    [HttpPost("{id:guid}/responses")]
    public async Task<IActionResult> AddResponse(Guid id, AddResponseRequest request)
    {
        var gameGrain = _cluster.GetGrain<IGameGrain>(id);

        await gameGrain.AddResponse(new Response(new ProductId(request.ProductId),
            new PromotionalPriceResponse(request.PromotionalPriceResponse.Value)));
        return Ok();
    }
}