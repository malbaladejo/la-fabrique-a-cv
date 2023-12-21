// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Logging;

internal class ConfigurationLoader
{
	private readonly ILogger logger;

	private const string Template = "-template";
	private const string Data = "-data";
	private const string Output = "-output";
	private const string Css = "-css";
	private const string Asset = "-asset";


	public ConfigurationLoader(ILogger logger)
	{
		this.logger = logger;
	}

	public Configuration? Load(string[] args)
	{
		var configuration = new Configuration();
		for (int i = 0; i < args.Length; i = i + 2)
		{
			if (args[i] == Template)
			{
				configuration.Template = args[i + 1];
				continue;
			}

			if (args[i] == Data)
			{
				configuration.Data = args[i + 1];
				continue;
			}

			if (args[i] == Output)
			{
				configuration.Output = args[i + 1];
				continue;
			}

			if (args[i] == Css)
			{
				configuration.CssFiles.Add(args[i + 1]);
				continue;
			}

			if (args[i] == Asset)
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
