using AspireToDo.Api.Infrastructure;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogger(builder.Configuration);
//builder.Services.AddStorage(builder.Configuration);
builder.AddStorage(builder.Configuration);
builder.AddRedisDistributedCache("cache");

builder.AddServiceDefaults();

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
});

builder.Services.AddCors();
//builder.Services.AddAuthentication().AddJwtBearer();
//builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
app.UseCors(static builder =>
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());

//app.UseHttpsRedirection();

app.UseWhen(context => !context.Request.Path.StartsWithSegments("/swagger"),
    builder => builder.UseHttpLogging());

//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();