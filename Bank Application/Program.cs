using Bank_Application.Data;
using Bank_Application.DesignPatterns;
using Bank_Application.DesignPatterns.Decorator;
using Bank_Application.Facade;
using Bank_Application.Factories;
using Bank_Application.Models;
using Bank_Application.Repositories;
using Bank_Application.Seeders;
using Bank_Application.services;
using Bank_Application.Services;
using Bank_Application.Services.Facade;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            );
        }));


builder.Services.AddScoped<IAccountTypeRepository, AccountTypeRepository>();

builder.Services.AddScoped<factoryAccountType>();
builder.Services.AddScoped<IAccountTypeRepository, AccountTypeRepository>();
builder.Services.AddScoped<IAccountTypeService, AccountTypeService>();
builder.Services.AddScoped<IAccountTypeFacade, AccountTypeFacade>();
builder.Services.AddScoped<IPasswordHasher<Employee>, PasswordHasher<Employee>>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IClientFacade, ClientFacade>();
builder.Services.AddScoped<IFeatureRepository, FeatureRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IClientAuthRepository, ClientAuthRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountFacadeService, AccountFacadeService>();
builder.Services.AddScoped<ISubAccountRepository, SubAccountRepository>();
builder.Services.AddScoped<ISubAccountService, SubAccountService>();
builder.Services.AddScoped<IAccountHierarchyService, AccountHierarchyService>();
builder.Services.AddScoped<IClientAccountRepository, ClientAccountRepository>();
builder.Services.AddScoped<ISubAccountRepository, SubAccountRepository>();
builder.Services.AddScoped<ISupportTicketRepository, SupportTicketRepository>();
builder.Services.AddScoped<ISupportTicketService, SupportTicketService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<FeatureService>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<FeatureService>();

builder.Services.AddScoped<IFeatureDecorator>(sp =>
{
    var service = sp.GetRequiredService<FeatureService>();
    var logger = sp.GetRequiredService<ILogger<FeatureLoggingDecorator>>();

    return new FeatureLoggingDecorator(service, logger);
});



builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var firstError = context.ModelState
                .Where(ms => ms.Value.Errors.Count > 0)
                .Select(ms => ms.Value.Errors.First().ErrorMessage)
                .FirstOrDefault();

            return new BadRequestObjectResult(new
            {
                message = firstError,
                status = 400
            });
        };
    });
// JWT
var jwt = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwt["Secret"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwt["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true
        };
    });
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("login", config =>
    {
        config.PermitLimit = 5;        
        config.Window = TimeSpan.FromMinutes(1); 
        config.QueueLimit = 0;
    });
});
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddScoped<IPasswordHasher<Employee>, PasswordHasher<Employee>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    AccountStatusSeeder.Seed(context);
}
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    EmployeeSeeder.Seed(db);
}



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRateLimiter();
app.UseAuthorization();

app.MapControllers();

app.Run();
