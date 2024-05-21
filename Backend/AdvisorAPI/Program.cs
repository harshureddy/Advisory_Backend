using AdvisorAPI.Data;
using AdvisorAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var cors_Sections = builder.Configuration.GetSection("Cors").GetChildren();

// Add services to the container.
builder.Services.AddDbContext<AdvisorContext>(options =>
               options.UseInMemoryDatabase("AdvisorDB"));
builder.Services.AddScoped<IAdvisorRepository, AdvisorRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region Cors Policy
foreach (var c in cors_Sections)
{
    if (!string.IsNullOrEmpty(c.Value))
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                              builder =>
                              {
                                  builder.WithOrigins(
                                      c.Value
                                      )
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowCredentials();
                              });
        });
    }
}
#endregion
var app = builder.Build();
#region enabling global cors policy

foreach (var c in cors_Sections)
{
    if (!string.IsNullOrEmpty(c.Value))
    {
        app.UseCors(x => x
        .WithOrigins(
          c.Value
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true) // allow any origin 
        .AllowCredentials());
    }
}
#endregion
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
