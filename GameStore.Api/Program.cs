using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
 
var connString =  builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSqlite<GameStoreContext>(connectionString: connString);

var app = builder.Build();

app.MapGamesEndpoints();

await app.MigrateDbAsync();

app.Run();
