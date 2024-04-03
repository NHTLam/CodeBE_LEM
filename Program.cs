using CodeBE_LEM.Models;
using CodeBE_LEM.Repositories;
using CodeBE_LEM.Services.AppUserService;
using CodeBE_LEM.Services.BoardService;
using CodeBE_LEM.Services.PermissionService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:3001");
            policy.WithMethods("POST", "PUT", "DELETE");
            policy.WithHeaders("Content-Type", "Authorization", "ngrok-skip-browser-warning");
        });
});
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<DataContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("dbconn")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Example add token: \"Authorization: Bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("AppSettings:Token")))
    };
});

builder.Services.AddScoped<IBoardService, BoardService>();
builder.Services.AddScoped<IBoardValidator, JobValidator>();
builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IUOW, UOW>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
