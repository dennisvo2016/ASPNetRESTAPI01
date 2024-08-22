using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

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

app.Run();
