// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

internal class NustasheDocumentRenderer : IDocumentRenderer
{
	private readonly ILogger<NustasheDocumentRenderer> logger;
	private readonly ITemplateBuidler templateBuidler;

	public NustasheDocumentRenderer(ILogger<NustasheDocumentRenderer> logger, ITemplateBuidler templateBuidler)
	{
		this.logger = logger;
		this.templateBuidler = templateBuidler;
	}

	public void Render(Configuration configuration)
	{
		try
		{
			logger.LogInformation("Reading Template");
			var template = templateBuidler.Build(configuration);

			logger.LogInformation("Reading data");
			var dataJson = File.ReadAllText(configuration.Data);

			logger.LogInformation("Parsing data");
			var data = JsonConvert.DeserializeObject(dataJson);

			logger.LogInformation("Rendering");
			var output = Nustache.Core.Render.StringToString(template, data);

			logger.LogInformation("Writting output");
			File.WriteAllText(configuration.Output, output);
		}
		catch (Exception ex)
		{
			logger.LogInformation($"Error: {ex}");
		}
	}
}
