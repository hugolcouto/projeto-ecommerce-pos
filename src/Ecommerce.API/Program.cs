using Ecommerce.Application;
using Ecommerce.Core;
using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.BackgroundJobs;
using Hangfire;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration).AddApplication().AddCore();

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

WebApplication? app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IRecurringJobManager recurringJobs =
        scope.ServiceProvider.GetService<IRecurringJobManager>()
        ?? throw new InvalidOperationException("Hangfire not properly configured");

    RecurringJob.AddOrUpdate<CanceledExpiredOrdersJob>(
        "expire-orders",
        job => job.ExecuteAsync(),
        Cron.Daily()
    );
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();
}

app.UseHangfireDashboard(
    "/hangfire",
    new DashboardOptions() { DashboardTitle = "EcommerceDev API Background Jobs" }
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
