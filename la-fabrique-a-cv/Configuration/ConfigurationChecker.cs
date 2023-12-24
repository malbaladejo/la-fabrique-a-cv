using la_fabrique_a_cv;
using Microsoft.Extensions.Logging;

internal class ConfigurationChecker : IWorkflowStep
{
    private readonly ILogger<ConfigurationChecker> logger;
    private readonly IDocumentationWritter documentationWritter;
    private bool isEnabled = true;

    public ConfigurationChecker(ILogger<ConfigurationChecker> logger, IDocumentationWritter documentationWritter)
    {
        this.logger = logger;
        this.documentationWritter = documentationWritter;
    }

    public bool Execute(Configuration configuration)
    {
        if (!this.isEnabled)
            return true;

        this.isEnabled = false;

        if (this.Check(configuration))
            return true;

        this.documentationWritter.Write();
        return false;
    }

    private bool Check(Configuration configuration)
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