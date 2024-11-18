using Entities.Colmena.NuGet.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Transversal.Entities.Colmena;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("_myAllowSpecificOrigins", builder => builder
    .SetIsOriginAllowed(origin => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ApiSettings apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
config.settings = apiSettings;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = JwtClaimColmena.issuer,
        ValidAudience = JwtClaimColmena.audience,
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(config.sKey)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

config.Environment = Enum.Parse<EnvironmentEnum>(config.settings.environment.Name);

if (config.settings?.connection != null)
{
    config.settings.connection.ConnLogin = config.Environment switch
    {
        EnvironmentEnum.Dev  => config.settings.connection.ConnD,
        EnvironmentEnum.Test => config.settings.connection.ConnT,
        EnvironmentEnum.Cert => config.settings.connection.ConnC,
        EnvironmentEnum.Prod => config.settings.connection.ConnP,
        _ => String.Empty,
    };
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("_myAllowSpecificOrigins");

app.Run();