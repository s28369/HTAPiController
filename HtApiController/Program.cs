using System.Data.SqlClient;
using HtApiController;
using HtApiController.Services.Animals;
using Microsoft.AspNetCore.Components.Sections;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddSingleton<IAnimalsService, AnimalService>();
}
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.MapControllers();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var animals = new List<Animal>();
string connectionString = "Server=localhost;Database=apbd;User Id=SA;Password=248652Alexey;";
SqlConnection connection = new SqlConnection(connectionString);
await connection.OpenAsync();

using SqlCommand command = new SqlCommand("SELECT * FROM Animal", connection);
using SqlDataReader reader = await command.ExecuteReaderAsync();


app.Run();
