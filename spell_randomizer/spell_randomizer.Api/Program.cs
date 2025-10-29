using Microsoft.AspNetCore.Mvc;
using spell_randomizer;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

Player? currentPlayer = null;

app.MapPost("/api/character/", (CharacterRequest request) => 
{
    if (string.IsNullOrWhiteSpace(request.Name))
        return Results.Content("<p>Character name cannot be empty</p>", "text/html");
    
    currentPlayer = new Player(request.Name);
    
    return Results.Redirect("/game.html");
});

// Load existing character
app.MapPost("/api/character/load", ([FromForm] string name) => 
{
    if (string.IsNullOrWhiteSpace(name))
        return Results.Content("<p>Character name cannot be empty</p>", "text/html");
    
    try
    {
        currentPlayer = Player.Load(name);
        return Results.Content(File.ReadAllText("wwwroot/game.html"), "text/html");
    }
    catch (Exception ex)
    {
        return Results.Content($"<p>Error loading character: {ex.Message}</p><br><a href='/'>Back</a>", "text/html");
    }
});

app.MapGet("/api/state", () => 
{
    if (currentPlayer == null)
        return Results.Content("<p>No character loaded</p>", "text/html");
    
    var html = $@"
        <div class='character-info'>
            <h2>{currentPlayer.Name}</h2>
            <div class='stats'>
                <div class='stat'>
                    <strong>Level:</strong> {currentPlayer.Level}
                </div>
                <div class='stat'>
                    <strong>Sorcery Points:</strong> {currentPlayer.MaxSorcPoints - currentPlayer.SorcPointsUsed} / {currentPlayer.MaxSorcPoints}
                </div>
                <div class='stat'>
                    <strong>Wild Magic Counter:</strong> {currentPlayer.WildMagicCounter}
                </div>
            </div>
            
            <div class='spell-slots'>
                <h3>Spell Slots</h3>
                <ul>
                    {string.Join("", Enumerable.Range(0, currentPlayer.SpellSlotsTotal.Length)
                        .Select(i => $"<li>Level {i + 1}: {currentPlayer.SpellSlotsTotal[i] - currentPlayer.SpellSlotsUsed[i]} / {currentPlayer.SpellSlotsTotal[i]}</li>"))}
                </ul>
            </div>
            
            <div class='cantrips'>
                <h3>Cantrips ({currentPlayer.Cantrips.Count} / {currentPlayer.CantripsTotal})</h3>
                <ul>
                    {string.Join("", currentPlayer.Cantrips.Select(c => $"<li>{c}</li>"))}
                </ul>
            </div>
            
            <div class='spells'>
                <h3>Spells Known ({currentPlayer.Spells.Count} / {currentPlayer.SpellsTotal})</h3>
                <ul>
                    {string.Join("", currentPlayer.Spells.Select(s => $"<li>{s}</li>"))}
                </ul>
            </div>
        </div>
    ";
    
    return Results.Content(html, "text/html");
});

app.Run();

record CharacterRequest(string Name);
