using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using questvault.Server.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<questvaultServerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("questvaultServerContext") ?? throw new InvalidOperationException("Connection string 'questvaultServerContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
