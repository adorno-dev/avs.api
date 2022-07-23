using System.Reflection;
using System.Text.Json.Serialization;
using AVS.API.Hubs;
using AVS.API.Repositories;
using AVS.API.Services;
using AVS.API.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserRefreshTokenRepository>();
builder.Services.AddScoped<ChatRepository>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ChatService>();

builder.Services.AddSignalR();

builder.Services
    .AddCors(o => o.AddPolicy("CorsPolicy", x => { x
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    }));

builder.Services
    .AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

builder.Services
    .AddAuthentication(x => {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; })
    .AddJwtBearer(x => {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters {
            IssuerSigningKey = new SymmetricSecurityKey(TokenSettings.GetSecret()),
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false }; 
        if (x.Events != null)
            x.Events.OnMessageReceived = context => {
                var accessToken = context.Request.Query["access_token"];
                var path = context.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/chatHub"))) {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "AVS.API",
        Description = "Avatar Virtual Style API",
        TermsOfService = new Uri("https://localhost:5000/terms"),
        Contact = new OpenApiContact {
            Name = "Example Contact",
            Url = new Uri("https://localhost:5000/contact")
        },
        License = new OpenApiLicense {
            Name = "Example License",
            Url = new Uri("https://localhost:5000/license")
        }
    });

    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseDefaultFiles();

app.UseStaticFiles();

app.MapControllers();

app.MapHub<ChatHub>("/chatHub");

app.Run();