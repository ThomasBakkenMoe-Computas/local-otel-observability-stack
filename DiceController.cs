using System.Net;
using Microsoft.AspNetCore.Mvc;

public class DiceController: ControllerBase
{
    private ILogger<DiceController> logger;

    public DiceController(ILogger<DiceController> logger)
    {
        this.logger = logger;
    }

    [HttpGet("/rolldice")]
    public List<int> RollDice(string player, int? rolls)
    {
        if(!rolls.HasValue)
        {
            logger.LogError("Missing rolls parameter");
            throw new HttpRequestException("Missing rolls parameter", null, HttpStatusCode.BadRequest);
        }

        var result = new Dice(1, 6).rollTheDice(rolls.Value);

        if (string.IsNullOrEmpty(player))
        {
            logger.LogInformation("Anonymous player rolled a dice with results: {result}", result);
        }
        else
        {
            logger.LogInformation("{player} rolled a dice with results: {result}", player, result);
        }

        return result;
    }
}