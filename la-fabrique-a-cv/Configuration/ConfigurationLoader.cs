// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Logging;

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
		for (int i = 0; i < args.Length; i = i + 2)
		{
			if (args[i] == ConfigurationFields.Template)
			{
				configuration.Template = args[i + 1];
				continue;
			}

			if (args[i] == ConfigurationFields.Data)
			{
				configuration.Data = args[i + 1];
				continue;
			}

			if (args[i] == ConfigurationFields.Output)
			{
				configuration.Output = args[i + 1];
				continue;
			}

			if (args[i] == ConfigurationFields.Css)
			{
				configuration.CssFiles.Add(args[i + 1]);
				continue;
			}

			if (args[i] == ConfigurationFields.Asset)
			{
				configuration.Assets.Add(args[i + 1]);
				continue;
			}

			this.logger.LogError($"ERROR [arg[{i}] - {args[i]}] not supported.");
			return null;
		}

		return configuration;
	}
}
