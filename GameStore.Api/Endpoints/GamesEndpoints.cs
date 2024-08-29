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

    private static readonly List<GameSummaryDto> games = [
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
        group.MapGet("/{id}", (int id, GameStoreContext dbContext) => {                        
            Game? game = dbContext.Games.Find(id);

            return game is null? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        })
        .WithName(GetGameEndpointName);

        //POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) => {
            

            Game game = newGame.ToEntity(); 
            
            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            GameDetailsDto gameDto = game.ToGameDetailsDto();

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, gameDto);
        });

        //PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) => {
            var existingGame = dbContext.Games.Find(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame)
            .CurrentValues
            .SetValues(updatedGame.ToEntity(id));

            dbContext.SaveChanges();

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
