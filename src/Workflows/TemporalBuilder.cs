// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.DependencyInjection;

using Options;
using Temporalio.Client;
using Temporalio.Extensions.Hosting;

/// <summary>
/// Provides a builder class for configuring and managing Temporal services, clients, and workers.
/// </summary>
internal sealed class TemporalBuilder : ITemporalBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TemporalBuilder"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="clientOptionsBuilder">The client options builder.</param>
    /// <param name="workerOptionsBuilder">The worker options builder.</param>
    public TemporalBuilder(
        string name,
        IServiceCollection serviceCollection,
        OptionsBuilder<TemporalClientConnectOptions> clientOptionsBuilder,
        ITemporalWorkerServiceOptionsBuilder workerOptionsBuilder)
    {
        Name = name;
        Services = serviceCollection;
        ClientOptionsBuilder = clientOptionsBuilder;
        WorkerOptionsBuilder = workerOptionsBuilder;
    }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public IServiceCollection Services { get; }

    /// <inheritdoc/>
    public OptionsBuilder<TemporalClientConnectOptions> ClientOptionsBuilder { get; }

    /// <inheritdoc/>
    public ITemporalWorkerServiceOptionsBuilder WorkerOptionsBuilder { get; }
}
