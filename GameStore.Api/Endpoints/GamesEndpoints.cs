using System;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = [
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

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        //GET /games
        group.MapGet("/", () => games);

        //GET /games/1
        group.MapGet("/{id}", (int id) => {
            GameDto? game = games.Find(g => g.Id == id);

            return game is null? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetGameEndpointName);

        //POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) => {
            

            Game game = newGame.ToEntity(); 
            game.Genre = dbContext.Genres.Find(newGame.GenreId);
            
            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            GameDto gameDto = game.ToDto();

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, gameDto);
        });

        //PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) => {
            var index = games.FindIndex(g => g.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(
                id, 
                updatedGame.Name, 
                updatedGame.Genre, 
                updatedGame.Price, 
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });

        //DELETE /games/1
        group.MapDelete("/{id}", (int id) => {
            var index = games.FindIndex(g => g.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games.RemoveAll(g => g.Id == id);

            return Results.NoContent();
        });

        return group;
    }
}
