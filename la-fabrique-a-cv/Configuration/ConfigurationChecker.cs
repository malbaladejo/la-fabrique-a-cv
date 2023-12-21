using Microsoft.Extensions.Logging;

internal class ConfigurationChecker
{
    private readonly ILogger<ConfigurationChecker> logger;

    public ConfigurationChecker(ILogger<ConfigurationChecker> logger)
    {
        this.logger = logger;
    }

    public bool Check(Configuration configuration)
    {
        logger.LogInformation("Checking configuration");

        if (configuration == null)
            return false;

        if (!CheckFile(configuration.Template, ConfigurationFields.Template))
            return false;

        if (!CheckFile(configuration.Data, ConfigurationFields.Data))
            return false;

        foreach (var item in configuration.CssFiles)
        {
            if (!CheckFile(item, ConfigurationFields.Template))
                return false;
        }

        foreach (var item in configuration.Assets)
        {
            if (!CheckFile(item, ConfigurationFields.Asset))
                return false;
        }

        return true;
    }

    private bool CheckFile(string? filePath, string attribute)
    {
        if (File.Exists(filePath))
            return true;

        logger.LogError($"{attribute}: {filePath} not found");
        return false;
    }
}