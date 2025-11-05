using CustomerManagement.API.Extensions;
using CustomerManagement.Core.Middleware;
using CustomerManagement.Domain;
using CustomerManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices()
    .AddApplicationValidators()
    .AddInfrastructure(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddApplicationMappers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.UseInfrastructure();

app.Run();
