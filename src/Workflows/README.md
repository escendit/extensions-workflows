# NuGet Package: Escendit.Extensions.Workflows

This NuGet package provides a streamlined integration with Temporal.io for .NET applications,
offering simplified configuration and dependency injection setup for building robust,
distributed workflow applications. It wraps Temporal's .NET SDK with opinionated defaults
and convenient extension methods.

Key features:

- Simplified Temporal client and worker configuration
- Built-in dependency injection integration
- Pre-configured JSON serialization with System.Text.Json
- OpenTelemetry tracing support out of the box
- Worker versioning and deployment management
- Keyed service registration for multi-tenant scenarios
- Production-ready defaults for common use cases

## Installation
To install this package, use the NuGet Package Manager Console:
```
shell PM> Install-Package Escendit.Extensions.Workflows
```
Or you can search for "Escendit.Extensions.Workflows"
in the NuGet Package Manager UI and install it from there.

## Usage

### Basic Setup

Register Temporal services in your `Program.cs` or `Startup.cs`:
```csharp
// Add Temporal hosted service with minimal configuration
builder
    .Services
    .AddTemporalHostedService("<task queue>")
    .ConfigureWorkerOptions(builder =>
    {
        builder
            .AddWorkflow<OrderWorkflow>()
            .AddActivity<CreateOrderActivity>()
            .AddActivity<ProcessOrderActivity>();
    });
```

### Advanced Configuration

Configure Temporal with custom settings:
```csharp
builder
    .Services
    .AddTemporalHostedService("<task queue>", "<target host>", "<namespace>", "<build id>")
    .ConfigureWorkerOptions(builder =>
    {
        builder
            .AddWorkflow<OrderWorkflow>()
            .AddActivity<CreateOrderActivity>()
            .AddActivity<ProcessOrderActivity>();
    });
```

### Using Keyed Services

For multi-tenant applications or multiple Temporal connections:
```csharp
// Register multiple Temporal clients
builder
    .Services
    .AddKeyedTemporalClient("<name>", "<target host?>", "<namespace?>");

builder
    .Services
    .AddKeyedTemporalClient("<name>", "<target host?>", "<namespace?>");

// Use in your services
public class OrderService
{
    private readonly ITemporalClient _clientA;
    private readonly ITemporalClient _clientB;

    public OrderService(
        [FromKeyedServices("<name 1>")] ITemporalClient clientA,
        [FromKeyedServices("<name 2>")] ITemporalClient clientB)
    {
        _clientA = clientA;
        _clientB = clientB;
    }
}
```

## Configuration

### Default Settings

The package uses the following defaults:

| Setting               | Default Value    | Description                                         |
|-----------------------|------------------|-----------------------------------------------------|
| `ClientTargetHost`    | `localhost:7233` | Temporal server address                             |
| `ClientNamespace`     | `default`        | Temporal namespace                                  |
| `UseWorkerVersioning` | `false`          | Enable worker versioning (true if buildId provided) |
| `DataConverter`       | System.Text.Json | JSON serialization using System.Text.Json           |
| `Tracing`             | Enabled          | OpenTelemetry tracing interceptors                  |

### Customizing Client Options

You can further customize the Temporal client after registration:
```csharp
builder
    .Services
    .AddTemporalHostedService("<name>")
    .ConfigureClientOptions(options =>
    {
        options.RetryConfig = new RetryConfig { InitialInterval = TimeSpan.FromSeconds(1), MaximumInterval = TimeSpan.FromMinutes(1), BackoffCoefficient = 2.0, MaximumAttempts = 3 };
    });
```

### Worker Versioning

Enable worker versioning for safe deployments:
```csharp
builder
    .Services
    .AddTemporalHostedService( "<task queue>", buildId: Environment.GetEnvironmentVariable("BUILD_ID"))
    .ConfigureWorkerOptions(builder =>
    {
        builder
            .AddWorkflow<OrderWorkflow>()
            .AddActivity<CreateOrderActivity>()
            .AddActivity<ProcessOrderActivity>();
    });
```

## Dependencies

This package depends on:

- `Temporalio.Extensions.Hosting` - Temporal .NET SDK hosting extensions
- `Temporalio.Extensions.OpenTelemetry` - OpenTelemetry integration for Temporal

These dependencies are automatically installed when you install this package.

## Contributing
If you find a bug or have a feature request, please create an issue in the GitHub repository.

To contribute code, fork the repository and submit a pull request.
Please ensure that your code follows the project's coding standards and is thoroughly tested.

## License
Licensed under Apache License 2.0. See the LICENSE file for the complete license text.
