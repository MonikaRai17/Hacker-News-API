
using HackerNewsServices.Service;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddTransient<IHackerNewsService, HackerNewsService>();
builder.Services.AddCors(p => p.AddPolicy("corspolicy", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corspolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
