// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.DependencyInjection;

using Options;
using Temporalio.Client;
using Temporalio.Extensions.Hosting;

/// <summary>
/// Represents a builder interface for constructing temporal workflow configurations or objects.
/// </summary>
public interface ITemporalBuilder
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; }

    /// <summary>
    /// Gets the services.
    /// </summary>
    /// <value>The services.</value>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Gets the client options builder.
    /// </summary>
    /// <value>The client options.</value>
    internal OptionsBuilder<TemporalClientConnectOptions> ClientOptionsBuilder { get; }

    /// <summary>
    /// Gets the worker builder.
    /// </summary>
    /// <value>The workflow builder.</value>
    internal ITemporalWorkerServiceOptionsBuilder WorkerOptionsBuilder { get; }
}
