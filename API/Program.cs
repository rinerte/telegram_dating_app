using DataLayer.Interfaces;
using DataLayer.Services;
using DataLayer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string connection = "";
try
{
    var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
    connection = config["connectionstring"];
    if (String.IsNullOrEmpty(connection)) throw new Exception();
} catch
{
    connection = Environment.GetEnvironmentVariable("CONNECTION_STRING");
}

builder.Services.AddDbContext<ProfileContext>(options => options.UseNpgsql(connection));
builder.Services.AddScoped<IProfileService, ProfileService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
            .AllowAnyHeader());
});
// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();
