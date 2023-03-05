using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopBridge.DataAccess.Data;
using ShopBridge.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(); //Swagger 

//Auth
builder.Services.AddAuthorization(opts =>
{
    opts.FallbackPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
});


builder.Services.AddAuthentication("Bearer").AddJwtBearer(opts =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Authentication:SecretKey"));

    opts.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
        ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

var sqlConnectionString = builder.Configuration.GetConnectionString("Default") ?? "null";

//Healthchecks
builder.Services.AddHealthChecks()
    .AddSqlServer(sqlConnectionString);

//Product Service
builder.Services.AddTransient<IProductService, ProductService>();

//DB settings, no tracking queries
builder.Services.AddDbContext<ShopDbContext>(options =>
    options.UseSqlServer(sqlConnectionString)
    .EnableSensitiveDataLogging()
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//Healthchecks
app.MapHealthChecks("/health").AllowAnonymous();

app.Run();
