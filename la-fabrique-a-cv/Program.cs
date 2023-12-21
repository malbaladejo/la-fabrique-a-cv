// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal partial class Program
{
	private static void Main(string[] args)
	{
		var host = CreateHost(args);
		var logger = host.Services.GetService<ILogger<Program>>();

		var documentWrite = host.Services.GetService<IDocumentationWritter>();
		logger.LogInformation("GO");
		try
		{
			if (args.Length == 0)
			{
				documentWrite.Write();
				return;
			}

			var configuration = host.Services.GetService<IConfigurationLoader>().Load(args);


			if (!host.Services.GetService<ConfigurationChecker>().Check(configuration))
			{
				documentWrite.Write();
				return;
			}

			EnsureOutput(configuration, logger);

			host.Services.GetService<IDocumentRenderer>().Render(configuration);
			host.Services.GetService<AssetExporter>().Export(configuration);
		}
		catch (Exception e)
		{
			logger.LogError(e.Message);
		}
		finally
		{
			logger.LogInformation("END");
		}
	}

	private static IHost CreateHost(string[] args)
	{
		var host = Host.CreateDefaultBuilder(args)
		.ConfigureServices(services =>
		{
			services.AddScoped<IConfigurationLoader, ConfigurationLoader>();
			services.AddScoped<ConfigurationChecker>();
			services.AddScoped<IDocumentationWritter, DocumentationWritter>();
			services.AddScoped<IDocumentRenderer, NustasheDocumentRenderer>();
			services.AddScoped<ITemplateBuidler, CssAwareTemplateBuidler>();

			services.AddScoped<AssetExporter>();

			services.AddLogging(logging =>
			{
				logging.AddConsole();
			});
		}).Build();

		return host;
	}

	private static void EnsureOutput(Configuration configuration, ILogger logger)
	{
		logger.LogInformation("Ensuring output directory");

		var outputDirectory = Path.GetDirectoryName(configuration.Output);
		if (!Directory.Exists(outputDirectory))
			Directory.CreateDirectory(outputDirectory);
	}
}
