// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stubble.Core.Builders;

internal class Program
{
	private static void Main(string[] args)
	{
		var host = CreateHost(args);

		var configurationLoader = host.Services
	}

	private static IHost CreateHost(string[] args)
	{
		var host = Host.CreateDefaultBuilder(args)
		.ConfigureServices(services =>
		{
			services.AddLogging(logging =>
			{
				logging.AddConsole();
			});
		}).Build();

		return host;
	}

	private static void Run(string[] args)
	{
		Console.WriteLine("GO");
		try
		{
			if (args.Length == 0)
			{
				WriteDocumentation();
				return;
			}

			var configuration = GetConfiguration(args);

			if (!CheckConfiguration(configuration))
			{
				WriteDocumentation();
				return;
			}

			EnsureOutput(configuration);

			Render(configuration);
		}
		finally
		{
			Console.WriteLine("END");
		}
	}

	private static void Render(Configuration configuration)
	{
		var stubble = new StubbleBuilder()
			.Configure(settings =>
			{
				settings.SetIgnoreCaseOnKeyLookup(true);
				settings.SetMaxRecursionDepth(512);
				settings.AddJsonNet();
			}).Build();
		try
		{

			Console.WriteLine("Reading Template");
			var template = File.ReadAllText(configuration.Template);

			Console.WriteLine("Reading data");
			var dataJson = File.ReadAllText(configuration.Data);

			Console.WriteLine("Parsing data");
			var data = JsonConvert.DeserializeObject(dataJson);

			Console.WriteLine("Rendering");
			var output = stubble.Render(template, data);

			Console.WriteLine("Writting output");
			File.WriteAllText(configuration.Output, output);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error: {ex}");
		}
	}

	private static void EnsureOutput(Configuration configuration)
	{
		var outputDirectory = Path.GetDirectoryName(configuration.Output);
		if (!Directory.Exists(outputDirectory))
			Directory.CreateDirectory(outputDirectory);
	}

	private static Configuration? GetConfiguration(string[] args)
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

			Console.WriteLine($"ERROR [arg[{i}] - {args[i]}] not supported.");
			return null;
		}

		return configuration;
	}

	private static void WriteDocumentation()
	{
		Console.WriteLine($"{Template}:\t [required] Path of the template file.");
		Console.WriteLine($"{Data}:\t\t [required] Path of the data file.");
		Console.WriteLine($"{Output}:\t [required] Path of the output file.");
		Console.WriteLine($"{Css}:\t\t [optional] [multiple] Path of css file to include in generated file.");
		Console.WriteLine($"{Asset}:\t\t [optional] [multiple] Pathof other file to copy to output.");
	}

	private static bool CheckConfiguration(Configuration configuration)
	{
		if (configuration == null)
			return false;

		if (!CheckFile(configuration.Template, Template))
			return false;

		if (!CheckFile(configuration.Data, Data))
			return false;

		foreach (var item in configuration.CssFiles)
		{
			if (!CheckFile(item, Template))
				return false;
		}

		foreach (var item in configuration.Assets)
		{
			if (!CheckFile(item, Asset))
				return false;
		}

		return true;
	}

	private static bool CheckFile(string filePath, string attribute)
	{
		if (File.Exists(filePath))
			return true;

		Console.WriteLine($"{attribute}: {filePath} not found");
		return false;
	}

}
