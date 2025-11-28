// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.DependencyInjection;

using Options;
using Temporalio.Client;
using Temporalio.Extensions.Hosting;

/// <summary>
/// Provides extension methods for configuring an <see cref="ITemporalBuilder"/> instance.
/// </summary>
public static class TemporalBuilderExtensions
{
    /// <summary>
    /// Provides extension methods for configuring an <see cref="ITemporalBuilder"/> instance.
    /// </summary>
    extension(ITemporalBuilder temporalBuilder)
    {
        /// <summary>
        /// Configure Client Options.
        /// </summary>
        /// <param name="configureOptions">The configure client options.</param>
        /// <returns>The updated temporal migrator builder.</returns>
        public ITemporalBuilder ConfigureClientOptions(
            Action<OptionsBuilder<TemporalClientConnectOptions>> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(temporalBuilder);
            ArgumentNullException.ThrowIfNull(configureOptions);
            configureOptions.Invoke(temporalBuilder.ClientOptionsBuilder);
            return temporalBuilder;
        }

        /// <summary>
        /// Configure Client Options.
        /// </summary>
        /// <param name="configureOptions">The configure client options.</param>
        /// <returns>The updated temporal migrator builder.</returns>
        public ITemporalBuilder ConfigureClientOptions(
            Action<TemporalClientConnectOptions> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(temporalBuilder);
            ArgumentNullException.ThrowIfNull(configureOptions);
            return ConfigureClientOptions(temporalBuilder, builder => builder.Configure(configureOptions));
        }

        /// <summary>
        /// Configure Worker Options.
        /// </summary>
        /// <param name="configureOptions">The configure options.</param>
        /// <returns>The updated temporal migrator builder.</returns>
        public ITemporalBuilder ConfigureWorkerOptions(
            Action<ITemporalWorkerServiceOptionsBuilder> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(temporalBuilder);
            ArgumentNullException.ThrowIfNull(configureOptions);
            configureOptions.Invoke(temporalBuilder.WorkerOptionsBuilder);
            return temporalBuilder;
        }
    }
}
