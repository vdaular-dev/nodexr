﻿namespace Nodexr.Api.Functions.IntegrationTests;

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nodexr.Api.Functions.Common;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

[SetUpFixture]
public class Testing
{
    private static IServiceScopeFactory _scopeFactory = null!;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var startup = new Startup();
        var host = new HostBuilder()
            .ConfigureWebJobs(startup.Configure)
            .Build();

        var services = host.Services;

        _scopeFactory = services.GetRequiredService<IServiceScopeFactory>();
    }

    public static async Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
    {
        using var scope = _scopeFactory.CreateScope();

        var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        return await handler.Handle(request, cancellationToken);
    }

    public static Task ResetState()
    {
        return Task.CompletedTask; // TODO
    }

    public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<INodexrContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}
