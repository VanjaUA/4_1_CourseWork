using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Warehouse.Api.Middleware;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Patterns;
using Warehouse.Application.Services;
using Warehouse.Domain.Interfaces;
using Warehouse.Infrastructure.Auth;
using Warehouse.Infrastructure.Data;
using Warehouse.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger Config with JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Warehouse API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// DbContext
builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

// Auth Helpers
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IPartnerService, PartnerService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Patterns
builder.Services.AddSingleton<IValuationStrategy, CurrentPriceValuationStrategy>();
builder.Services.AddScoped<IStockObserver, LowStockObserver>(); // Scoped because it uses Repo
builder.Services.AddScoped<IStockSubject, StockSubject>();

// JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

var app = builder.Build();

// Seed Admin
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<WarehouseDbContext>();
    // context.Database.Migrate(); // Auto-migrate if needed, but manual is requested.
    
    // Seed Admin
    var userRepo = services.GetRequiredService<IRepository<Warehouse.Domain.Entities.User>>();
    var config = services.GetRequiredService<IConfiguration>();
    var hasher = services.GetRequiredService<IPasswordHasher>();
    
    var adminEmail = config["AdminSeed:Email"];
    if (!string.IsNullOrEmpty(adminEmail)) 
    {
        var exists = (await userRepo.FindAsync(u => u.Email == adminEmail)).Any();
        if (!exists)
        {
            var admin = new Warehouse.Domain.Entities.User
            {
                Id = Guid.NewGuid(),
                Email = adminEmail,
                PasswordHash = hasher.Hash(config["AdminSeed:Password"]!),
                Role = Warehouse.Domain.Enums.Role.Admin
            };
            await userRepo.AddAsync(admin);
        }
    }
}

// Middleware Pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
