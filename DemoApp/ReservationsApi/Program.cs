using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using ReservationsApi.Api;
using ReservationsApi.Api.CreateRegistration;
using ReservationsApi.Api.GetRegistration;
using ReservationsApi.Contracts.Validators;
using ReservationsApi.Data;
using ReservationsApi.Middlewares;
using ReservationsApi.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHealthChecks()
    .AddUrlGroup(new Uri("https://github.com/mholec/tech-reservations"), "GitHub Specification");

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicies.Default, policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddDbContext<AppDbContext>(x => x.UseInMemoryDatabase("demo"));
builder.Services.AddMediatR(typeof(Program).Assembly);

//validation
builder.Services.AddFluentValidation(x=>
{
    x.RegisterValidatorsFromAssemblyContaining<RegistrationCreateValidator>();
    x.RegisterValidatorsFromAssemblyContaining<RegistrationCreateApiValidator>();
});
builder.Services.AddScoped<ApiValidationContext>();

// cache
builder.Services.AddMemoryCache();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));





WebApplication app = builder.Build();

app.UseMiddleware<ApiExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.CreateRegistration();
app.GetRegistration();

app.MapHealthChecks("healthz", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

// integration tests needs Program public
public partial class Program { }