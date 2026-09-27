using Library.Application;
using Library.Persistence;
using Library.Persistence.Seeds;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await DataBaseSeeder.SeedAsync(app.Services);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
