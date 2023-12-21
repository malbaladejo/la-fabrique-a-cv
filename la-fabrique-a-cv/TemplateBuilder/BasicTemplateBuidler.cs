// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Logging;

internal class BasicTemplateBuidler : ITemplateBuidler
{
	private readonly ILogger<BasicTemplateBuidler> logger;

	public BasicTemplateBuidler(ILogger<BasicTemplateBuidler> logger)
	{
		this.logger = logger;
	}

	public string Build(Configuration configuration)
	{
		logger.LogInformation("Reading template");
		return File.ReadAllText(configuration.Template);
	}
}
