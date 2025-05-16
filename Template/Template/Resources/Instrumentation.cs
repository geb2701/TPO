using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Template.Resources;

/// <summary>
/// It is recommended to use a custom type to hold references for
/// ActivitySource and Instruments. This avoids possible type collisions
/// with other components in the DI container.
/// </summary>
public class Instrumentation : IDisposable
{
    internal const string ActivitySourceName = "Examples.AspNetCore";
    internal const string MeterName = "Examples.AspNetCore";
    private readonly Meter meter;

    public Instrumentation()
    {
        string? version = typeof(Instrumentation).Assembly.GetName().Version?.ToString();
        this.ActivitySource = new ActivitySource(ActivitySourceName, version);
        this.meter = new Meter(MeterName, version);
        this.CommandHitCounter = this.meter.CreateCounter<long>("command.hits", description: "The number of hits to commands.");
    }

    public ActivitySource ActivitySource { get; }

    public Counter<long> CommandHitCounter { get; }

    public void Dispose()
    {
        this.ActivitySource.Dispose();
        this.meter.Dispose();
    }
}