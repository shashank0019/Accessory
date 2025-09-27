using AccessoryCreation.BusinessLogic;
using AccessoryCreation.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Add services to container
builder.Services.AddControllers();

// 🔹 Add Swagger (API documentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 Add Dependency Injection for BusinessLogic & DataAccess
builder.Services.AddScoped<AccessoryBusinessLogic>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    string connStr = config.GetConnectionString("DefaultConnection");
    return new AccessoryBusinessLogic(connStr);
});

builder.Services.AddScoped<AccessoryDataAccess>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    string connStr = config.GetConnectionString("DefaultConnection");
    return new AccessoryDataAccess(connStr);
});

var app = builder.Build();

// 🔹 Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
