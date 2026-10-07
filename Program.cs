using Scalar.AspNetCore; 
using EditorialApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<AutoresServices>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapScalarApiReference(options =>
{
    options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();