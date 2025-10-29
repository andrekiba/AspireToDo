using AspireToDo.Api.Infrastructure;
using Microsoft.AspNetCore.HttpLogging;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogger(builder.Configuration);
//builder.Services.AddStorage(builder.Configuration);
builder.AddStorage(builder.Configuration);
builder.AddRedisDistributedCache("cache");

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.Services.AddProblemDetails();

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
});

builder.Services.AddCors();
//builder.Services.AddAuthentication().AddJwtBearer();
//builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

//app.UseHttpsRedirection();
app.UseCors(static builder =>
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());

app.UseWhen(context => !context.Request.Path.StartsWithSegments("/scalar"),
    static builder => builder.UseHttpLogging());

//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();