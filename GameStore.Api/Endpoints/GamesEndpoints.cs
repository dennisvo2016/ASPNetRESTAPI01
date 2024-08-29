using System;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        //GET /games
        group.MapGet("/", async (GameStoreContext dbContext) => {
            return await dbContext.Games
            .Include(g => g.Genre)//include Genre object inside Game object
            .Select(g => g.ToGameSummaryDto())
            .AsNoTracking()
            .ToListAsync();//improve the performance of the app
        });

        //GET /games/1
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) => {                        
            Game? game = await dbContext.Games.FindAsync(id);

            return game is null? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        })
        .WithName(GetGameEndpointName);

        //POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) => {
            Game game = newGame.ToEntity(); 
            
            dbContext.Games.Add(game);
            
            await dbContext.SaveChangesAsync();

            GameDetailsDto gameDto = game.ToGameDetailsDto();

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, gameDto);
        });

        //PUT /games/1
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) => {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame)
            .CurrentValues
            .SetValues(updatedGame.ToEntity(id));

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        //DELETE /games/1
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) => {
            await dbContext.Games.Where(g => g.Id == id).ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return group;
    }
}
