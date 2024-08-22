using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
    new (
        0, "item 0", "name item 0", 5.00M, new DateOnly(2020,9,30)
    ),
    new (
        1, "item 1", "name item 1", 10.00M, new DateOnly(2020,10,30)
    ),
    new (
        2, "item 2", "name item 2", 20.00M, new DateOnly(2020,11,30)
    ),
    new (
        3, "item 3", "name item 3", 30.00M, new DateOnly(2020,12,30)
    )
];

//GET /games
app.MapGet("games", () => games);

//GET /games/1
app.MapGet("games/{id}", (int id) => games.Find(g => g.Id == id))
.WithName(GetGameEndpointName);

//POST /games
app.MapPost("games", (CreateGameDto newGame) => {
    GameDto game = new (
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id}, game);
});

//PUT /games
app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame) => {
    var index = games.FindIndex(g => g.Id == id);

    if (index != -1)
    {
        games[index] = new GameDto(
            id, 
            updatedGame.Name, 
            updatedGame.Genre, 
            updatedGame.Price, 
            updatedGame.ReleaseDate
        );
    }
    else
    {
        return Results.BadRequest();
    }

    return Results.NoContent();
});

app.Run();
