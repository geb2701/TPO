using QueryKit.Configuration;

namespace Tpo.Resources.QueryKitUtilities;

public class CustomQueryKitConfiguration : QueryKitConfiguration
{
    public CustomQueryKitConfiguration(Action<QueryKitSettings> configureSettings = null)
        : base(settings =>
        {
            // configure custom global settings here
            // settings.EqualsOperator = "eq";

            // By default, QueryKit will throw an error if it doesn't recognize a property name,
            // If you want to loosen the reigns here a bit, you can set AllowUnknownProperties to true in your config.
            // When active, unknown properties will be ignored in the expression resolution.
            // https://github.com/pdevito3/QueryKit?tab=readme-ov-file#allow-unknown-properties
            settings.AllowUnknownProperties = true;

            configureSettings?.Invoke(settings);
        })
    {
    }
}