using AzDevOps.Service.Application.Configurations;
using AzDevOps.Service.Application.Interfaces.HttpClients;
using AzDevOps.Service.Application.Interfaces.Wiql;
using AzDevOps.Service.Infrastructure.Builders.Wiql;
using AzDevOps.Service.Infrastructure.HttpClients;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions<AzDevOpsConfig>("AzDevOps");
builder.Services.AddHttpClient<IWorkItemsHttpClient, WorkItemsHttpClient>(
    "WitHttpClient",
    (serviceProvider, clientConfig) => {
        var azConfig = serviceProvider.GetRequiredService<AzDevOpsConfig>();
        clientConfig.BaseAddress = new Uri($"https://dev.azure.com/{azConfig.Organization}/{azConfig.Project}/_apis");
        clientConfig.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "", azConfig.PAT))));
    });
builder.Services.AddTransient<IWiqlBuilder, WiqlBuilder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
