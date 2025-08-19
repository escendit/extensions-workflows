// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides default values for Temporal client configuration constants.
/// </summary>
public static class TemporalDefaults
{
    /// <summary>
    /// Default Host And Port.
    /// </summary>
    public const string ClientTargetHost = "localhost:7233";

    /// <summary>
    /// Default Namespace.
    /// </summary>
    public const string ClientNamespace = "default";
}
