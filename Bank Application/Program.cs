using Bank_Application.Data;

using Bank_Application.design_pattern.Observe;
using Bank_Application.DesignPatterns;

using Bank_Application.DesignPatterns.Decorator;
using Bank_Application.Facade;
using Bank_Application.Factories;
using Bank_Application.Hubs;
using Bank_Application.Models;
using Bank_Application.Repositories;
using Bank_Application.Seeders;
using Bank_Application.services;
using Bank_Application.Services;
using Bank_Application.Services.Facade;
using Bank_Application.Strategies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
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
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ISubAccountRepository, SubAccountRepository>();
builder.Services.AddScoped<IClientAccountRepository, ClientAccountRepository>();
builder.Services.AddScoped<ISupportTicketRepository, SupportTicketRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ITransactionLogRepository, TransactionLogRepository>();
builder.Services.AddScoped<IRecommendationRepository, RecommendationRepository>();

// ================= Services =================
builder.Services.AddScoped<IAccountTypeService, AccountTypeService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ISubAccountService, SubAccountService>();
builder.Services.AddScoped<IAccountHierarchyService, AccountHierarchyService>();
builder.Services.AddScoped<ISupportTicketService, SupportTicketService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<ITransactionService,TransactionService>();
builder.Services.AddScoped<IAccountResolverService, AccountResolverService>();
builder.Services.AddScoped<FeatureService>();

builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ITransactionSubject, TransactionNotifier>();
builder.Services.AddScoped<ITransactionObserver, NotificationObserver>();
builder.Services.AddScoped<IAccountTransactionRepository, AccountTransactionRepository>();
builder.Services.AddScoped<IClientAccountTransactionRepository, ClientAccountTransactionRepository>();
builder.Services.AddScoped<ITransactionLogRepository, TransactionLogRepository>();

builder.Services.AddSignalR();

builder.Services.AddScoped<IAccountTypeFacade, AccountTypeFacade>();
builder.Services.AddScoped<IClientFacade, ClientFacade>();
builder.Services.AddScoped<IAccountFacadeService, AccountFacadeService>();

builder.Services.AddScoped<factoryAccountType>();
builder.Services.AddScoped<FeatureService>();
builder.Services.AddScoped<IPasswordHasher<Employee>, PasswordHasher<Employee>>();
builder.Services.AddScoped<IClientAuthRepository, ClientAuthRepository>();
builder.Services.AddScoped<IFeatureRepository, FeatureRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRecommendationStrategy, SavingsRecommendationStrategy>();
builder.Services.AddScoped<IRecommendationStrategy, PremiumRecommendationStrategy>();

builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<ITransactionLogRepository, TransactionLogRepository>();
builder.Services.AddScoped<IRecommendationRepository, RecommendationRepository>();
builder.Services.AddScoped<ITransactionApprovalService, TransactionApprovalService>();
builder.Services.AddScoped<ITransactionReportService, TransactionReportService>();

builder.Services.AddHostedService<LoanSchedulerService>();
builder.Services.AddScoped<LoanService>();
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
    });

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
        opt.Events = new JwtBearerEvents
        {

            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken) &&
                    (context.HttpContext.Request.Path.StartsWithSegments("/notificationHub") ))
                   

                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
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
builder.Services.AddSignalR();



QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddControllers();


builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLiveServer", policy =>
    {
        policy
            .WithOrigins("http://127.0.0.1:5500")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowLiveServer");

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationHub>("/supportTicketHub");
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    AccountStatusSeeder.Seed(context);
    EmployeeSeeder.Seed(context);
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    EmployeeSeeder.Seed(db);
   
}
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

   
    TransactionTypeSeeder.Seed(context);
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
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();


app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");

app.Run();
