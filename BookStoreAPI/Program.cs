using BookStoreAPI.Filters;
using BookStoreAPI.Mapper;
using BookStoreAPI.Middleware;
using BookStoreAPI.Options;
using BookStoreAPI.Service;
using BookStoreAPI.Service.Implementation;
using BookStoreAPI.Service.Interface;
using BookStoreAPI.Util;
using BookStoreClassLibrary.Core.Entities;
using BookStoreClassLibrary.Core.Repository;
using BookStoreClassLibrary.Core.Security;
using BookStoreClassLibrary.Core.UnitOfWork;
using BookStoreClassLibrary.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Configure swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Book Store API",
        Version = "v1",

    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.OperationFilter<AuthResponsesOperationFilter>();
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

});

// Configure dependency injection
builder.Services.AddScoped<DbContext, BookStoreDbContext>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IGenericRepository<Author, Guid>, GenericRepository<Author, Guid>>();
builder.Services.AddScoped<IGenericRepository<Book, Guid>, GenericRepository<Book, Guid>>();
builder.Services.AddScoped<IGenericRepository<RefreshToken, Guid>, GenericRepository<RefreshToken, Guid>>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ExceptionMiddleware>();

// Configure auto mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configure DataBase
IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
builder.Services.AddSingleton<AuditingInterceptor>();
builder.Services.AddDbContext<BookStoreDbContext>((sp, options) =>
{
    var auditingInterceptor = sp.GetService<AuditingInterceptor>();
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString")).AddInterceptors(auditingInterceptor);
}
);

// Configure identity
builder.Services.AddIdentity<User, IdentityRole>(o =>
{
    o.User.RequireUniqueEmail= true;
})
.AddEntityFrameworkStores<BookStoreDbContext>()
.AddDefaultTokenProviders();

// Configure JWT
var jwtConfig = configuration.GetSection("JwtConfig");
var secretKey = jwtConfig["SecretKey"];
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig["ValidIssuer"],
        ValidAudience = jwtConfig["ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
    };
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
      .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
      .RequireAuthenticatedUser()
      .Build();
});

// Jwt Options
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.JwtSection));
builder.Services.ConfigureOptions<JwtOptionsSetup>();

// Configure Logger
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Store API v1");
    });
}

app.UseHttpsRedirection();

app.UseHttpLogging();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();


