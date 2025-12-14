using Bank_Application.Data;
using Bank_Application.DesignPatterns;
using Bank_Application.DesignPatterns.Decorator;
using Bank_Application.Facade;
using Bank_Application.Factories;
using Bank_Application.Models;
using Bank_Application.Repositories;
using Bank_Application.Seeders;
using Bank_Application.Services;
using Bank_Application.Services.Facade;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

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
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountFacadeService, AccountFacadeService>();
builder.Services.AddScoped<ISubAccountRepository, SubAccountRepository>();
builder.Services.AddScoped<ISubAccountService, SubAccountService>();
builder.Services.AddScoped<IAccountHierarchyService, AccountHierarchyService>();
builder.Services.AddScoped<IClientAccountRepository, ClientAccountRepository>();
builder.Services.AddScoped<ISubAccountRepository, SubAccountRepository>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
