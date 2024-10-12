// See https://aka.ms/new-console-template for more information
using la_fabrique_a_cv;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

internal partial class Program
{
    private static async Task Main(string[] args)
    {

        // -watch -template D:\dev\la-fabrique-a-cv\templates\theme1\cv-template.html -data D:\dev\la-fabrique-a-cv\templates\data.json -output D:\dev\la-fabrique-a-cv\output\theme1\index.html -css D:\dev\la-fabrique-a-cv\templates\theme1\cv-style.css  -css D:\dev\la-fabrique-a-cv\templates\theme1\cv-style-blue.css -asset D:\dev\la-fabrique-a-cv\templates\picture.jpg
        var host = CreateHost(args);
        var logger = host.Services.GetService<ILogger<Program>>();

        var documentWrite = host.Services.GetService<IDocumentationWritter>();
        var steps = host.Services.GetService<IEnumerable<IWorkflowStep>>().ToArray();
        var fileWatcher = host.Services.GetService<IFileWatcher>();

        logger.LogInformation("GO");
        try
        {
            if (args.Length == 0)
            {
                documentWrite.Write();
                return;
            }

            if (args.Contains(ConfigurationFields.New))
            {
                CreateEmptyConfigurationFile(logger, args);
                return;
            }

            var configuration = GetConfiguration(host, args);

            Execute(logger, configuration, steps);

            if (configuration.WatchFiles)
            {
                fileWatcher.StartWatch(configuration, steps);
                while (true)
                {
                    logger.LogInformation("Press q to quit.");
                    var input = Console.ReadLine();
                    if (input == "q")
                        break;
                    else
                    {
                        logger.LogInformation("Do you want to quit ? (y/n)");
                        input = Console.ReadLine();
                        if (input == "y")
                            break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
        }
        finally
        {
            (fileWatcher as IDisposable)?.Dispose();
            logger.LogInformation("END");
        }
    }

    // TODO mettre ca ailleurs
    public static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private static void CreateEmptyConfigurationFile(ILogger logger, string[] args)
    {
        var emptyConfiguration = new Configuration();
        var json = JsonSerializer.Serialize(emptyConfiguration, jsonSerializerOptions);

        if (args.Length == 1)
        {
            logger.LogError($"You must put the config file path after {ConfigurationFields.Config}");
            return;
        }

        File.WriteAllText(args[1], json);
    }

    private static void Execute(ILogger logger, Configuration configuration, IEnumerable<IWorkflowStep> steps)
    {
        logger.LogInformation($"Render begin.");
        foreach (var step in steps)
        {
            if (!step.Execute(configuration))
                break;
        }
        logger.LogInformation($"Render end.");
    }

    private static Configuration? GetConfiguration(IHost host, string[] args)
        => host.Services.GetService<IConfigurationLoader>().Load(args);

    private static IHost CreateHost(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
        .ConfigureServices(services =>
        {
            services.AddScoped<IConfigurationLoader, ConfigurationLoader>();
            services.AddScoped<IDocumentationWritter, DocumentationWritter>();
            services.AddScoped<ITemplateBuidler, CssAwareTemplateBuidler>();


            services.AddScoped<IFileWatcher, FileWatcher>();

            services.AddScoped<IWorkflowStep, ConfigurationChecker>();
            services.AddScoped<IWorkflowStep, OuptBuilder>();
            services.AddScoped<IWorkflowStep, NustasheDocumentRenderer>();
            services.AddScoped<IWorkflowStep, AssetExporter>();

            services.AddLogging(logging =>
            {
                logging.AddConsole();
            });
        }).Build();

        return host;
    }
}
