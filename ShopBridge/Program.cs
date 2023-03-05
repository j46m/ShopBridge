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

//Swagger
builder.Services.AddSwaggerGen(opts =>
{
    var title = "Shop Bridge";
    var description = "Simple API to showcase .NET 6";

    opts.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = title,
        Description = description
    });

    opts.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = title,
        Description = description
    });
});

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

//API versioning
builder.Services.AddApiVersioning(opts =>
{
    opts.AssumeDefaultVersionWhenUnspecified = true;
    opts.DefaultApiVersion = new(1, 0);
    opts.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(opts =>
{
    opts.GroupNameFormat = "'v'VVV";
    opts.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts =>
    {
        opts.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        opts.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
    });
}

app.UseHttpsRedirection();

//Auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//Healthchecks
app.MapHealthChecks("/health").AllowAnonymous();

app.Run();
