// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class ConfigurationLoader : IConfigurationLoader
{
    private readonly ILogger<ConfigurationLoader> logger;

    public ConfigurationLoader(ILogger<ConfigurationLoader> logger)
    {
        this.logger = logger;
    }

    public Configuration? Load(string[] args)
    {
        this.logger.LogInformation("Reading configuration");

        var configuration = new Configuration();

        if (args.Contains(ConfigurationFields.Config))
            configuration = this.LoadFromConfigFile(args);
        else
            this.ReadInlineConfiguration(args, configuration);

        configuration.Initialize();
        return configuration;
    }

    private Configuration LoadFromConfigFile(string[] args)
    {
        if (args.Length < 2)
        {
            logger.LogError($"You must put the config file path after {ConfigurationFields.Config}");
            return new Configuration();
        }

        var configFilePath = args[1];

        if (!this.CheckFile(configFilePath, ConfigurationFields.Config))
            return new Configuration();

        var json = File.ReadAllText(configFilePath);
        var configuration = JsonSerializer.Deserialize<Configuration>(json, Program.jsonSerializerOptions);

        if (string.IsNullOrEmpty(configuration.WorkingDirectory))
            configuration.WorkingDirectory = Path.GetDirectoryName(configFilePath);

        return configuration;
    }

    private bool CheckFile(string? filePath, string attribute)
    {
        if (File.Exists(filePath))
            return true;

        logger.LogError($"{attribute}: {filePath} not found");
        return false;
    }

    private void ReadInlineConfiguration(string[] args, Configuration configuration)
    {
        var i = 0;
        while (i < args.Length)
        {
            if (args[i] == ConfigurationFields.WorkingDirectory)
            {
                configuration.WorkingDirectory = args[i + 1];
                i = i + 2;
                continue;
            }

            if (args[i] == ConfigurationFields.Template)
            {
                configuration.Template = args[i + 1];
                i = i + 2;
                continue;
            }

            if (args[i] == ConfigurationFields.Data)
            {
                configuration.Data = args[i + 1];
                i = i + 2;
                continue;
            }

            if (args[i] == ConfigurationFields.Output)
            {
                configuration.Output = args[i + 1];
                i = i + 2;
                continue;
            }

            if (args[i] == ConfigurationFields.Css)
            {
                configuration.CssFiles.Add(args[i + 1]);
                i = i + 2;
                continue;
            }

            if (args[i] == ConfigurationFields.Asset)
            {
                configuration.Assets.Add(args[i + 1]);
                i = i + 2;
                continue;
            }

            if (args[i] == ConfigurationFields.Watch)
            {
                configuration.WatchFiles = true;
                i = i + 1;
                continue;
            }

            if (args[i] == ConfigurationFields.ClearOutput)
            {
                configuration.ClearOutput = true;
                i = i + 1;
                continue;
            }

            this.logger.LogError($"ERROR [arg[{i}] - {args[i]}] not supported.");
        }
    }
}
