using BookManagementAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var server = Environment.GetEnvironmentVariable("DB_SERVER") ?? builder.Configuration["DB_SERVER"];
var database = Environment.GetEnvironmentVariable("DB_DATABASE") ?? builder.Configuration["DB_DATABASE"];
var passwd = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? builder.Configuration["DB_PASSWORD"];
var ConnectionString = $"Server={server};Database={database};User Id=sa;Password={passwd};TrustServerCertificate=True";
builder.Services.AddDbContext<AppDataContext>(x => x.UseSqlServer(ConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI();
}
using(var scope=app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDataContext>();
    db.Database.Migrate();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
