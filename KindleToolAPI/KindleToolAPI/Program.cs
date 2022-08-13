using KindleToolAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IClippingsService, ClippingsService>();
builder.Services.AddCors(c => c.AddPolicy("Cors", policy =>
{
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
    policy.AllowAnyOrigin();
    policy.AllowAnyMethod();
}
));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(option =>
    {
        option.DocumentTitle = "KindleToolAPI";
    });
}

app.UseHttpsRedirection();

app.UseCors("Cors");

app.UseAuthorization();

app.MapControllers();

app.Run();
