// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.DependencyInjection;

using System.Text.Json;
using Extensions;
using Logging;
using Options;
using Temporalio.Client;
using Temporalio.Common;
using Temporalio.Converters;
using Temporalio.Extensions.Hosting;
using Temporalio.Extensions.OpenTelemetry;
using Temporalio.Worker;

/// <summary>
/// Provides extension methods for configuring and adding Temporal services to an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Provides extension methods for configuring and adding Temporal services to an <see cref="IServiceCollection"/>.
    /// </summary>
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Add Temporal Hosted Service.
        /// </summary>
        /// <param name="name">The task queue name.</param>
        /// <param name="clientTargetHost">The client target host.</param>
        /// <param name="clientNamespace">The client namespace.</param>
        /// <param name="buildId">The build id.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public ITemporalBuilder AddTemporalHostedService(
            string name,
            string clientTargetHost = TemporalDefaults.ClientTargetHost,
            string clientNamespace = TemporalDefaults.ClientNamespace,
            string? buildId = null)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(name);
            var clientOptionsBuilder = services
                .AddKeyedTemporalClient(name, clientTargetHost, clientNamespace)
                .Configure(options =>
                {
                    var serializerOptions = new JsonSerializerOptions();
                    options.DataConverter = new DataConverter(new DefaultPayloadConverter(serializerOptions), new DefaultFailureConverter());
                    options.Interceptors = [new TracingInterceptor()];
                });

            var workerOptionsBuilder = services
                .AddHostedTemporalWorker(clientTargetHost, clientNamespace, name, new WorkerDeploymentOptions
                {
                    UseWorkerVersioning = buildId is not null,
                    DefaultVersioningBehavior = VersioningBehavior.Unspecified,
                    Version = buildId is null
                        ? null
                        : new WorkerDeploymentVersion(name, buildId),
                })
                .ConfigureOptions(
                    options =>
                    {
                        var serializerOptions = new JsonSerializerOptions();

                        options.Interceptors = [new TracingInterceptor()];

                        if (options.ClientOptions is null)
                        {
                            return;
                        }

                        options.ClientOptions.DataConverter = new DataConverter(new DefaultPayloadConverter(serializerOptions), new DefaultFailureConverter());
                        options.ClientOptions.Interceptors = [new TracingInterceptor()];
                    },
                    disallowDuplicates: true);

            return new TemporalBuilder(name, services, clientOptionsBuilder, workerOptionsBuilder);
        }

        /// <summary>
        /// Add Keyed Temporal Client.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="clientTargetHost">The client target host.</param>
        /// <param name="clientNamespace">The client namespace.</param>
        /// <returns>The updated service collection.</returns>
        /// <exception cref="ArgumentException">Throws exception when service key is invalid type.</exception>
        public OptionsBuilder<TemporalClientConnectOptions> AddKeyedTemporalClient(
            string name,
            string? clientTargetHost = null,
            string? clientNamespace = null)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(name);

            services.TryAddKeyedSingleton<ITemporalClient>(name, (serviceProvider, _) =>
                TemporalClient.CreateLazy(serviceProvider.GetRequiredService<IOptionsMonitor<TemporalClientConnectOptions>>().Get(name)));

            var builder = services.AddOptions<TemporalClientConnectOptions>(name);

            if (clientTargetHost is not null || clientNamespace is not null)
            {
                builder.Configure(options =>
                {
                    options.TargetHost = clientTargetHost;

                    if (clientNamespace is not null)
                    {
                        options.Namespace = clientNamespace;
                    }
                });
            }

            builder.Configure<IServiceProvider>((options, provider) =>
            {
                if (provider.GetService<ILoggerFactory>() is { } loggerFactory)
                {
                    options.LoggerFactory = loggerFactory;
                }
            });

            return builder;
        }
    }
}
